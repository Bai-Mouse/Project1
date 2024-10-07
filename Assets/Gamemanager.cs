
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
    public float spawnInterval = .3f,MaxEnemyCount=8, CurrentEnemyNum, EnemyCount;
    public float spawnTimer = 0f;
    public float myscore;
    public TextMeshProUGUI TextMeshPro,LevelText,Timer, BulletNum;
    public GameObject Floor;
    public GameObject[] Floors;
    public float Level;
    // Start is called before the first frame update
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CurrentEnemyNum = 0;
        Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        myTimer += Time.deltaTime;
        Timer.text = "Timer:"+(60 - Mathf.Floor(myTimer)).ToString();
        if (60 - myTimer<10)
        {
            Timer.color = Color.red;
        }
        else
        {
            Timer.color = Color.black;
        }
        BulletNum.text = "Bullet#:" + Player.GetComponent<Player>().BulletCount.ToString();
        if (Player.GetComponent<Player>().BulletCount < 3)
        {
            BulletNum.color = Color.red;
        }
        else
        {

            BulletNum.color = Color.black;  
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval&& EnemyCount< MaxEnemyCount)
        {
            spawnTimer = 0;
            EnemyCount++;
            GameObject newEnemy = Instantiate(Enemy);
            newEnemy.transform.position += new Vector3(Random.Range(-10, 10),0,0);
            CurrentEnemyNum++;

        }
        myscore = Player.GetComponent<Player>().score;
        TextMeshPro.text = "Score: "+myscore.ToString();
    }
    private void FixedUpdate()
    {
        myFixedTimer += Time.fixedDeltaTime;
    }
    public void NewRoom()
    {
        Floor = Instantiate(Floors[Random.Range(0, Floors.Length)]);
        Floor.transform.position = transform.position;
        LevelText.text = "";
        MaxEnemyCount = Random.Range(8,12+ Level);
        CurrentEnemyNum = 0;
        EnemyCount = 0;
        Floor.SetActive(true);
        Transform cam = Player.GetComponent<Player>().cam.transform;
        Vector3 vector3 = Player.transform.position;
        Player.transform.position = new Vector3(0, 100, 0);
        cam.position = Player.transform.position+(cam.transform.position- vector3);
    }
    public void EndRoom()
    {

        Destroy(Floor); 
        Level++;
        LevelText.text = "LEVEL: " + Level.ToString();

    }
}
