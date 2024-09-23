
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
    public float speed = 0.5f;
    bool Jumpable, Gethit;
    Collider2D box;
    public Vector3 initialScale;
    public LayerMask ground;
    Color InitialColor;
    void Start()
    {
        box = GetComponent<Collider2D>();
        initialScale = transform.localScale;
        health = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        InitialColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = Player.transform;
        
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
        if (transform.position.x > PlayerTransform.transform.position.x)
        {
            transform.Translate(-speed * Time.fixedDeltaTime, 0, 0, Space.World);
        }
        else
        {
            transform.Translate(speed * Time.fixedDeltaTime, 0, 0, Space.World);
        }
        Jump();
    }
    public void gethit(Vector2 d, float s)
    {
        health--;
        gethittime = 0.2f;
        Gethit = true;
        transform.localScale += new Vector3(2, 2, 0);
        if (health <= 0)
        {
            Destroy(gameObject);
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
        }
        if (rb.velocity.y<0 && !Jumpable && box.IsTouchingLayers(ground))
        {
            Jumpable = true;
        }
        

    }
}
