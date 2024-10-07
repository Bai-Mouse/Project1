
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public SpriteRenderer spriteRenderer;
    public float gethittime;
    Rigidbody2D Rigidbody;
    Transform PlayerTransform;
    Player Player;
    Rigidbody2D rb;
    int direction;
    public float speed = 0.5f;
    bool Jumpable, Gethit, Turnable,OnGround;
    Collider2D box;
    public Vector3 initialScale;
    public LayerMask ground;
    Color InitialColor;
    ParticleSystem ParticleSystem;
    Gamemanager gamemanager;
    public GameObject[] Buffs;
    public AudioClip[] audioClips;
    public AudioSource AudioSource;
    public Eyes[] eyes;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        speed += Random.Range(2, 4.00f);
        gamemanager = FindObjectOfType<Gamemanager>();
        box = GetComponent<Collider2D>();
        initialScale = transform.localScale;
        health = 5;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        InitialColor = spriteRenderer.color;
        ParticleSystem = GetComponent<ParticleSystem>();
        Turnable = true;
        AudioSource.pitch = Random.Range(0.60f, 1.40f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0)return;
        
        if (gethittime > 0)
        {
            gethittime-=Time.deltaTime;
            spriteRenderer.color = Color.white;
            if (gethittime <= 0)
            {
                Gethit = false;
                transform.localScale = initialScale;
                spriteRenderer.color = InitialColor;
            }
        }
        if (Gethit)
        {
            transform.localScale += (initialScale - transform.localScale) / 20;
        }
    }
    private void FixedUpdate()
    {
        if (health <= 0) return;
        PlayerTransform = Player.transform;
        foreach (Eyes e in eyes)
        {
            e.SetDirection((PlayerTransform.position- transform.position).normalized,transform.eulerAngles.z);
        }

        if (Turnable)
        if (transform.position.x > PlayerTransform.position.x)
        {
                direction = -1;
        }
        else
        {
                direction = 1;
        }
        transform.Translate(direction * speed * Time.fixedDeltaTime, 0, 0, Space.World);
        
        Jump();
        
    }
    private IEnumerator Die(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            print(GameObject.FindGameObjectsWithTag("Collectible").Length);
            
            gamemanager.CurrentEnemyNum--;
           
            if (gamemanager.CurrentEnemyNum == 0)
            {
                gamemanager.EndRoom();
            }
            Destroy(gameObject);
        }
    }
    public void gethit(Vector2 d, float s,float damage)
    {
        if (health <= 0) return;
        transform.localScale = new Vector3(2, 2, 0);
        ParticleSystem.Play();
        health-= damage;
        gethittime = 0.2f;
        Gethit = true;
        Turnable = true;

        if (health <= 0)
        {
            AudioSource.clip = audioClips[1];
            AudioSource.Play();
            box.enabled = false;
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            transform.localScale = new Vector3(3, 3, 0);
            Player.score++;
            spriteRenderer.enabled = false;
            if (Random.Range(0, 100) <= 70)
            {
                int i = Random.Range(0, Buffs.Length);
                if (Player.health <= Player.maxhealth / 3)
                {
                    i = 2;
                }
                if (Player.BulletCount <= 3)
                {
                    i = 0;
                }
                if (gamemanager.myTimer >= 50)
                {
                    i = 1;
                }
                Instantiate(Buffs[i]).transform.position = transform.position;

            }
            foreach (Eyes e in eyes)
            {
                e.transform.parent.gameObject.SetActive(false);
                e.gameObject.SetActive(false);
            }
            StartCoroutine(Die(0.5f));
        }
        else
        {
            
            AudioSource.clip = audioClips[0];
            AudioSource.Play();
        }
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = d * s;

    }
    public void gethit(Vector2 d, float s)
    {
        gethit(d,s,1);
    }
    public void Jump()
    {
        if (Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.5f, ground))
        {
            OnGround = true;
            Turnable = true;
        }
        else
        {
            OnGround = false;
        }
        if (Physics2D.Raycast(transform.position, new Vector2(direction, 0), 1, ground) && Jumpable)
        {
            rb.velocity = Vector2.up * 25f;
            AudioSource.clip = audioClips[2];
            AudioSource.Play();
            Jumpable = false;
            
        }
        if (Random.Range(0,100)<15 && Vector2.Distance(transform.position,PlayerTransform.transform.position) <= 8 && transform.position.y < PlayerTransform.position.y-1 && Jumpable)
        {
            rb.velocity = Vector2.up * 25f;
            AudioSource.clip = audioClips[2];
            AudioSource.Play();
            Jumpable = false;
            
        }
        if (rb.velocity.y<0 && !Jumpable && OnGround)
        {
            Jumpable = true;
            Turnable=true;
            
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health <= 0) return;
        if (collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.y>=-1)
            collision.gameObject.GetComponent<Player>().gethit(collision.transform.position-transform.position,20);
        }
        if(gethittime > 0)
        if (collision.gameObject.tag == "Collectible")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                if (collision.gameObject.GetComponent<Enemy>().health <= 0)
                {
                    return;
                }
                collision.gameObject.GetComponent<Enemy>().gethit(collision.transform.position - transform.position, 10);

            }

        }
    }
}
