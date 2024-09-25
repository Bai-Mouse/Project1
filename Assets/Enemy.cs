
using System.Collections;
using System.Collections.Generic;
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
    bool Jumpable, Gethit, Turnable;
    Collider2D box;
    public Vector3 initialScale;
    public LayerMask ground;
    Color InitialColor;
    ParticleSystem ParticleSystem;
    void Start()
    {
        box = GetComponent<Collider2D>();
        initialScale = transform.localScale;
        health = 4;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        InitialColor = spriteRenderer.color;
        ParticleSystem = GetComponent<ParticleSystem>();
        Turnable = true;
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
        if(Turnable)
        if (transform.position.x > PlayerTransform.transform.position.x)
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
            Destroy(gameObject);
        }
    }
    public void gethit(Vector2 d, float s)
    {
        if (health <= 0) return;
        transform.localScale += new Vector3(1, 1, 0);
        ParticleSystem.Play();
        health--;
        gethittime = 0.2f;
        Gethit = true;
        transform.localScale += new Vector3(2, 2, 0);
        
        if (health <= 0)
        {
            Player.score++;
            spriteRenderer.enabled = false;
            StartCoroutine(Die(0.5f));
        }
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = d * s;

    }
    public void Jump()
    {
        if (Random.Range(0,100)<10 && transform.position.y < PlayerTransform.position.y-1 && Jumpable)
        {
            rb.velocity = Vector2.up * 30f;
            Jumpable = false;
            Turnable = false;
        }
        if (rb.velocity.y<0 && !Jumpable && box.IsTouchingLayers(ground))
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
            collision.gameObject.GetComponent<Player>().gethit(collision.transform.position-transform.position,20);
        }
    }
}
