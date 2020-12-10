using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static bool GameIsOver;

    public GameObject gameObjectUI;


    void Start()
    {
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver = true)
            return;

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        GameIsOver = true;
        Debug.Log("GameOver");
    }

    public void WinLevel()
    {
        GameIsOver = true;
    }
}
