using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBall : MonoBehaviour
{
    public Vector3 force;
    Rigidbody rb;

    public int damage;

    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddRelativeForce(force, ForceMode.Impulse);
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //Explode
        animator.SetTrigger("Explode");
        //Stop movement
        rb.velocity = Vector3.zero;

        Destroy(gameObject, 0.33f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            Monster enemy = other.GetComponent<Monster>();
            enemy.TakeDamage(damage, transform.position);
            enemy.Explode(transform.position);
        }
    }
}
