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

    void Update()
    {
        if (countDown <= 0f)
        {
            StartCoroutine(SpawnWave()); // timer
            countDown = waveIntermission;
        }
        countDown -= Time.deltaTime;
        waveCountText.text = "Wave: " + Mathf.Round(countDown).ToString();
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
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
