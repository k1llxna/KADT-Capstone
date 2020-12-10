using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Structure : MonoBehaviourPun
{
    public int maxHealth;
    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
