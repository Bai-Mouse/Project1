using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public SpriteRenderer spriteRenderer;
    public float gethittime;
    Rigidbody2D Rigidbody;

    void Start()
    {
        health = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = Color.red;
        if (gethittime > 0)
        {
            gethittime-=Time.deltaTime;
            spriteRenderer.color = Color.white;
        }
    }
    public void gethit(Vector2 d, float s)
    {
        health--;
        gethittime = 0.2f;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = d * s;
    }
}
