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
        transform.Translate(Direction.normalized);
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
        if (collision.gameObject.tag == "Collectible")
        {
            collision.gameObject.GetComponent<Enemy>().gethit(Direction,speed);
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            Player.GetComponent<Player>().score++;
            Destroy(gameObject);
        }
    }

}
