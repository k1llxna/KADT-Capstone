using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject monster;
    public Transform[] spawnPoint;
    bool spawning = true;

    int spawnNum = 0;

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

            Monster newMonster = Instantiate(monster, spawnPoint[random].position, Quaternion.identity).GetComponent<Monster>();

            newMonster.damage += spawnNum * 1;
            newMonster.maxHealth += spawnNum * 5;
            newMonster.health = newMonster.maxHealth;
            newMonster.UpdateHealthBar();

            spawnNum++;

            yield return new WaitForSeconds(2f);
        }
    }
}
