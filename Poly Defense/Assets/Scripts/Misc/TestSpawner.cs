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
        for (int i = 0; i < 4; i++)
        {
            int random = Random.Range(0, 3);
            int random2 = Random.Range(3, 6);
            int random3 = Random.Range(6, 9);

            Main newMonster = Instantiate(monster, spawnPoint[random].position, Quaternion.identity).GetComponent<Main>();
            Main newMonster2 = Instantiate(monster, spawnPoint[random2].position, Quaternion.identity).GetComponent<Main>();
            Main newMonster3 = Instantiate(monster, spawnPoint[random3].position, Quaternion.identity).GetComponent<Main>();

            yield return new WaitForSeconds(2f);
        }
    }
}
