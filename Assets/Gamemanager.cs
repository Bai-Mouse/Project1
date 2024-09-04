using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    //public Time passed variable;
    public float myTimer = 0;
    public float myFixedTimer=0;

    public GameObject Enemy;
    public float spawnInterval = .5f;
    public float spawnTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myTimer += Time.deltaTime;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            Debug.Log("Spawn");
            Instantiate(Enemy);
        }
        
    }
    private void FixedUpdate()
    {
        myFixedTimer += Time.fixedDeltaTime;
    }
}
