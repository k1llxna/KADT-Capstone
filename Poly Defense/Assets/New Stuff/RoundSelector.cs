using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSelector : MonoBehaviour
{

    public Round[] waves;    

    public Round GetWave(int wave)
    {
        return waves[wave];
    }

    public int GetWaves()
    {
        return waves.Length;
    }
}
