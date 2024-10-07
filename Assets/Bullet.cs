using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 Direction;
    public float speed;
    Rigidbody2D Rigidbody;
    float Timer;
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = Direction*speed;

    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (collision.gameObject.tag == "Collectible")
        {
            if (collision.GetComponent<Enemy>())
            {
                if (collision.gameObject.GetComponent<Enemy>().health <= 0)
                {
                    return;
                }
                collision.gameObject.GetComponent<Enemy>().gethit(Direction, speed / 1.5f);
                
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.tag !="Buff"&& collision.gameObject.tag != "Bullet")
        Destroy(gameObject);
    }

}
