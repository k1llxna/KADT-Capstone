﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject monster;
    public Transform[] spawnPoint;
    bool spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {   
        while (spawning)
        {
            int random = Random.Range(0, spawnPoint.Length);

            Main newMonster = Instantiate(monster, spawnPoint[random].position, Quaternion.identity).GetComponent<Main>();

            yield return new WaitForSeconds(2f);

            spawning = false;
        }
    }
}
