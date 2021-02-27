using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    public int[] amountToSpawn;
    public GameObject[] monsters;

    int spawnNum = 0;

    GameManager gm;

    List<GameObject> enemiesToSpawn = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    IEnumerator Spawn()
    {     
        foreach (GameObject enemy in enemiesToSpawn)
        {
            int random = Random.Range(0, spawnPoint.Length);

            Monster newMonster = Instantiate(enemy, spawnPoint[random].position, Quaternion.identity).GetComponent<Monster>();

            newMonster.health = newMonster.maxHealth;
            newMonster.UpdateHealthBar();

            gm.AddEnemy(newMonster);

            yield return new WaitForSeconds(1f);
        }
    }

    public void SetMobs()
    {
        //monsters = monsters_;
        enemiesToSpawn.Clear();
        spawnNum = 0;

        //Add all the enemies needed to spawn to a list
        foreach (int amount in amountToSpawn)
        {
            for (int i = 0; i < amount; i++)
            {
                enemiesToSpawn.Add(monsters[spawnNum]);
            }

            spawnNum++;
        }

        //Scramble the list to spawn randomly
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            GameObject temp = enemiesToSpawn[i];
            int randomIndex = Random.Range(i, enemiesToSpawn.Count);
            enemiesToSpawn[i] = enemiesToSpawn[randomIndex];
            enemiesToSpawn[randomIndex] = temp;
        }
    }

    public int GetAmountOfEnemies()
    {
        return enemiesToSpawn.Count;
    }

    public void StartSpawning()
    {
        spawnNum = 0;
        StartCoroutine(Spawn());
    }
}
