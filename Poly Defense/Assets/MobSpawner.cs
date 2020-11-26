using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject enemy;


    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            foreach (Transform t in spawnPoints)
            {
                Instantiate(enemy, t.position, Quaternion.identity, transform);
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
