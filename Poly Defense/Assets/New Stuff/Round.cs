using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    public int regularSpawners;
    public int hardSpawners;   

    public GameObject[] regularSpawnerEnemies;
    public GameObject[] hardSpawnerEnemies;

    public int[] amountOfRegularEnemies;
    public int[] amountOfHardEnemies;
}
