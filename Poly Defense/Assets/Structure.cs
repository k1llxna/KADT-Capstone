﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int maxHealth;
    int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
