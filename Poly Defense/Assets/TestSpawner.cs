using System.Collections;
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
            int random = Random.Range(0, 3);


            Instantiate(monster, spawnPoint[random].position, Quaternion.identity);

            yield return new WaitForSeconds(2f);
        }
    }
}
