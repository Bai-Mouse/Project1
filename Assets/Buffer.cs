using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    float Timer;
    Gamemanager gamemanager;
    // Update is called once per frame
    private void Start()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
    }
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer > 5)
        {
            Destroy(gameObject);
        }
        if ((gamemanager.myFixedTimer) % 0.44f<=0.02f)
        {
            transform.localScale = new Vector3(2, 2, 2);

        }

        transform.localScale += (Vector3.one - transform.localScale)/20;
    }
}
