using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public float maxDis = 0.2f;
    Vector2 direction;
    // Start is called before the first frame update

    public void SetDirection(Vector2 d)
    {
        direction = d.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = maxDis * direction;
    }
}
