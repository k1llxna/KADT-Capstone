using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money; // static vars transfer between scenes
    public int startMoney = 400;

    void Start()
    {
        Money = startMoney;
    }
}
