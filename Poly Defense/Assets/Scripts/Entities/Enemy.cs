using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int startHealth = 100;
    public float health;

    public int value = 10;
    public GameObject deathEffect;

    public float startSpeed;
    [HideInInspector]
    public float speed;

    [Header("Unity Stuff")]
    public Image healthBar;


    void Start()
    {
        health = startHealth;
        startSpeed = speed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }

    void Die()
    {
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        PlayerStats.Money += value;
        Destroy(gameObject);
    }
}

/*
        ///////////////////////    AI stuff    /////////////////////////////

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
               // transform.forward = vel.normalized;

        var desiredVelocity = target.transform.position - transform.position;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity;

        var steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
     */
