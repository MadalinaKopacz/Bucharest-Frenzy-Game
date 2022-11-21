using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    //public GameObject[] monsters;
    public GameObject monster;
    int randomSpawnPoints;

    public static bool spawnAllowed;

    private int birdNumber;

    // Start is called before the first frame update
    void Start()
    {
        birdNumber = GameObject.FindGameObjectsWithTag("bird").Length;
        print(birdNumber);
        spawnAllowed = true;
        InvokeRepeating("SpawnMonster", 0f, 10f);
    }

    void SpawnMonster()
    {
        birdNumber = GameObject.FindGameObjectsWithTag("bird").Length;
        if (birdNumber >= 3)
            spawnAllowed = false;
        print(birdNumber);
        if (spawnAllowed)
        {
            randomSpawnPoints = Random.Range(0, spawnPoints.Length);
            Instantiate(monster, spawnPoints[randomSpawnPoints].position, Quaternion.identity);
        }
    }
}
