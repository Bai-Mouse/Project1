using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float jumpforce = 50f;
    public float score;
    public float bulletspeed = 30f;
    public float CameraBuffer = 1;
    public int BulletNum, BulletCount;
    float gethittime;

    public GameObject gun,bullet;
    public Rigidbody2D rb;
    SpriteRenderer SpriteRenderer;
    Color OriginalColor;
    Collider2D box;
    public Camera cam;
    public LayerMask ground;
    bool Jumping,Jumpable;
    public bool Gethit;
    public Eyes[] eyes;
    public float health = 20;
    public float maxhealth;
    public Slider slider;
    Gamemanager gamemanager;
    public GameObject ENDGAME;
    public AudioClip[] audioClips;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0, 100, 0);
        AudioSource = GetComponent<AudioSource>();
        BulletNum = 1;
        BulletCount = 50;
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
        cam = FindAnyObjectByType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<CircleCollider2D>();
        Jumpable = true;
        maxhealth = health;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        OriginalColor = SpriteRenderer.color;
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {



        Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - gun.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.transform.eulerAngles = new Vector3(0, 0, angle);
        foreach(Eyes e in eyes)
        {
            e.SetDirection(direction.normalized);
        }

        if (Mathf.Abs(angle) > 90)
            gun.transform.localScale = new Vector3(1, -1, 1);
        else
            gun.transform.localScale = new Vector3(1, 1, 1);

        if (BulletCount>0&&Input.GetKeyDown(KeyCode.Mouse0))
        {
            BulletCount--;
            rb.velocity += direction.normalized * -2.3f;
            AudioSource.clip = audioClips[0];
            AudioSource.Play();
            for (int i=0; i< BulletNum; i++)
            {

                GameObject b = Instantiate(bullet, gun.transform.position, transform.rotation);
                Bullet bulletscript = b.GetComponent<Bullet>();
                bulletscript.speed = bulletspeed;
                bulletscript.Direction = direction.normalized;
                b.transform.Translate(bulletscript.Direction * 1.5f);
                b.transform.Translate(new Vector2(Mathf.Cos((angle + 90) * Mathf.Deg2Rad), Mathf.Sin((angle + 90) * Mathf.Deg2Rad)) * Random.Range(-0.1f* (BulletNum-1), 0.1f*(BulletNum - 1)));

            }
            
        }
        

        

    }
    public void Jump()
    {
        AudioSource.clip = audioClips[4];
        AudioSource.Play();
        rb.velocity = Vector2.up * jumpforce;

    }
    void FixedUpdate()
    {
        cam.transform.position += new Vector3((transform.position.x - cam.transform.position.x) / CameraBuffer, (transform.position.y - cam.transform.position.y) / CameraBuffer, 0);
        transform.Translate(Dir().x * speed,0,0,Space.World);
        if(Input.GetAxis("Jump") > 0.5f && Jumpable )
        {
            Jump();
            Jumpable = false;
        }
        if (rb.velocity.y <= 0 && !Jumpable && box.IsTouchingLayers(ground))
        {
            Jumpable = true;
        }
        if (gethittime>0)
        {
            gethittime -= Time.fixedDeltaTime;
            SpriteRenderer.color = Color.white;
            cam.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            if (gethittime <= 0)
            {
                SpriteRenderer.color = OriginalColor;
                Gethit = false;
                if (health <= 0)
                {
                    gameObject.SetActive(false);
                    AudioSource.clip = audioClips[1];
                    AudioSource.Play();
                    ENDGAME.SetActive(true);
                }

            }
        }
        if(transform.position.y <= -100)
        {

            gamemanager.NewRoom();
        }
        if (gamemanager.myTimer >= 60)
        {
            if(health>0)
            gethit(600);
        }
    }
    public Vector3 Dir()
    {
        
        float x = Input.GetAxis("Horizontal");

        return new Vector3(x, 0, 0);
    }
    public void gethit(Vector2 d, float s)
    {
        
        //ParticleSystem.Play();
        health--;
        gethittime = 0.2f;
        Gethit = true;
        slider.value = health;
        AudioSource.clip = audioClips[2];
        AudioSource.Play();

        if (rb.velocity.y !< -1)
        rb.velocity = d * s;

    }
    public void gethit(float h)
    {

        //ParticleSystem.Play();
        health-=h;
        gethittime = 0.2f;
        Gethit = true;
        slider.value = 0;
        AudioSource.clip = audioClips[2];
        AudioSource.Play();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Buff")
        {
            print(collision.name);
            switch(collision.name){
                case "ExtraBullet(Clone)":
                    BulletCount+=10;
                    
                    AudioSource.clip = audioClips[7];
                    AudioSource.Play();
                    break;
                case "ExtraTime(Clone)":
                    AudioSource.clip = audioClips[5];
                    AudioSource.Play();
                    gamemanager.myTimer-=3;
                    if (gamemanager.myTimer < 0)
                    {
                        gamemanager.myTimer = 0;
                    }
                        break;
                case "Health(Clone)":
                    AudioSource.clip = audioClips[6];
                    AudioSource.Play();
                    health +=3;
                    if (health > maxhealth)
                    {
                        health = maxhealth;
                    }
                    slider.value = health;
                    break;
            }
            Destroy(collision.gameObject);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(rb.velocity.y<-1&& collision.transform.position.y<transform.position.y)
        if (collision.gameObject.tag == "Collectible")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                if (collision.gameObject.GetComponent<Enemy>().health <= 0)
                {
                    return;
                }
                collision.gameObject.GetComponent<Enemy>().gethit(collision.transform.position - transform.position, 5,2);
                    rb.velocity = Vector2.up * jumpforce/2;
                    AudioSource.clip = audioClips[3];
                    AudioSource.Play();
                }

        }
    }

}
