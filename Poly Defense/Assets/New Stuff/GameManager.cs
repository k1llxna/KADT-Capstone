using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int round;
    public int maxRounds;

    int totalEnemiesThisRound;
    int enemiesKilledThisRound;

    public int counter = 60;

    List<Monster> enemiesAlive = new List<Monster>();

    public TestSpawner[] spawners;

    RoundSelector roundSelector;

    //Enemy Spawn Points
    //Set spawn points to correct enemies to spawn

    public enum GameState
    {
        START,
        WAITING,
        STARTROUND,
        FIGHTING,
        ENDROUND,
        GAMEOVER
    }

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = 0;
        round = 1;

        spawners = FindObjectsOfType<TestSpawner>();
        roundSelector = GetComponent<RoundSelector>();

        maxRounds = roundSelector.GetWaves();

        StartCoroutine(CoreLoop());
    }

    // Update is called once per frame
    IEnumerator CoreLoop()
    {
        while (true)
        {
            
            switch (gameState)
            {
                //Get Ready to Start First Round
                case GameState.START:
                    //wait for player to ready up;
                    if (Input.GetKeyDown("k"))
                    {
                        gameState = GameState.STARTROUND;
                    }
                    break;
                //Get Ready to start Subsequent Rounds
                case GameState.WAITING:
                    //Countdown from 60, or until players readys up, then switch to fighting
                    yield return new WaitForSeconds(1f);

                    counter--;

                    if (counter < 0)
                        gameState = GameState.STARTROUND;

                    if (Input.GetKey("k"))
                    {
                        gameState = GameState.STARTROUND;
                    }

                    break;
                //Set Up the Round
                case GameState.STARTROUND:
                    //Get Next Round Rules
                    Round newRound = roundSelector.GetWave(round - 1);
                    totalEnemiesThisRound = 0;
                    //Set Spawner To correct Enemies
                    foreach (TestSpawner spawner in spawners)
                    {
                        spawner.amountToSpawn = newRound.amountOfRegularEnemies;
                        spawner.monsters = newRound.regularSpawnerEnemies;
                        spawner.SetMobs();

                        totalEnemiesThisRound += spawner.GetAmountOfEnemies();

                        spawner.StartSpawning();
                    }
                    //Start The Round
                    gameState = GameState.FIGHTING;
                    break;
                
                case GameState.FIGHTING:
                    //Get all enemies and when all enemies are killed, end the round
                    if (enemiesAlive.Count == 0 && enemiesKilledThisRound >= totalEnemiesThisRound)
                        gameState = GameState.ENDROUND;
                    break;

                case GameState.ENDROUND:
                 
                    yield return new WaitForSeconds(3f);

                    round++;
                    enemiesAlive.Clear();
                    enemiesKilledThisRound = 0;
                    counter = 60;

                    if(round > maxRounds)
                    {
                        gameState = GameState.GAMEOVER;
                        break;
                    }

                    gameState = GameState.WAITING;
                    break;

                case GameState.GAMEOVER:
                    //Do stuff
                    break;
            }

            yield return null;
        }


    }



    public void AddEnemy(Monster enemy)
    {
        enemiesAlive.Add(enemy);
    }

    public void KillEnemy(Monster enemy)
    {
        enemiesKilledThisRound++;
        enemiesAlive.Remove(enemy);
    }
}
