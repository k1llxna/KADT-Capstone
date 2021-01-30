using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Structure : MonoBehaviourPun
{
    public int maxHealth;
    public int health;

    AudioSource audio;

    protected virtual void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        audio.PlayOneShot(audio.clip);

        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
