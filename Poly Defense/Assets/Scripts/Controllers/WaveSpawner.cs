using UnityEngine;
using System.Collections; // coroutine included
using UnityEngine.UI; // Text

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab; // what to spawn
    public Transform spawnPoint;

    public float waveIntermission = 5.5f;
    public Text waveCountText;


    private float countDown = 2f;
    private int waveNumber = 0;

    public Transform Enemies;

    void Update()
    {
        if (countDown <= 0f)
        {
            StartCoroutine(SpawnWave()); // timer
            countDown = waveIntermission;
        }
        countDown -= Time.deltaTime;

        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);

        waveCountText.text = string.Format("{0:00.0}", countDown);
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;

        Debug.Log("Wave incoming");
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.8f);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, Enemies);
    }
}
