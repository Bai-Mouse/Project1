using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Eyes : MonoBehaviour
{
    public float maxDis = 0.2f;
    public Transform eyeball;
    Vector2 direction;
    float angle;
    Gamemanager gamemanager;
    // Start is called before the first frame update
    private void Start()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
            maxDis = 0.3f;
    }
    public void SetDirection(Vector2 d)
    {
        direction = d.normalized;
        eyeball = transform.parent;
    }
    public void SetDirection(Vector2 d,float z)
    {
        direction = d.normalized;
        angle = z;
    }
    // Update is called once per frame
    void Update()
    {
        if ((gamemanager.myFixedTimer) % 0.44f <= 0.02f)
        {
            transform.localPosition = Vector3.zero;

        }
        transform.localPosition += (Vector3)(maxDis * direction- (Vector2)transform.localPosition)/20;
        eyeball = transform.parent;
        eyeball.localEulerAngles= new Vector3(0,0,0- angle);
    }
}
