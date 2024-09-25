using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float jumpforce = 50f;
    public float score;
    public float bulletspeed = 30f;
    public float CameraBuffer = 1;

    public GameObject gun,bullet;
    public Rigidbody2D rb;
    Collider2D box;
    public Camera cam;
    public LayerMask ground;
    bool Jumping,Jumpable;
    public Eyes[] eyes;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindAnyObjectByType<Camera>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<Collider2D>();
        Jumpable = true;
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rb.velocity += direction.normalized * -2.3f;
            GameObject b = Instantiate(bullet,gun.transform.position,transform.rotation);
            Bullet bulletscript = b.GetComponent<Bullet>();
            bulletscript.speed = bulletspeed;
            bulletscript.Direction = direction.normalized;
            
        }
        

        

    }
    public void Jump()
    {
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
    }
    public Vector3 Dir()
    {
        
        float x = Input.GetAxis("Horizontal");

        return new Vector3(x, 0, 0);
    }

    
    
}
