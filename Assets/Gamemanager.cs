using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    //public Time passed variable;
    public float myTimer = 0;
    public float myFixedTimer=0;
    public GameObject Player;
    public GameObject Enemy;
    public float spawnInterval = .5f;
    public float spawnTimer = 0f;
    public float myscore;
    public TextMeshProUGUI TextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
            GameObject newEnemy = Instantiate(Enemy);
            newEnemy.transform.position += new Vector3(Random.Range(-10, 10),0,0);


        }
        myscore = Player.GetComponent<Player>().score;
        TextMeshPro.text = "Score: "+myscore.ToString();
    }
    private void FixedUpdate()
    {
        myFixedTimer += Time.fixedDeltaTime;
    }
}
