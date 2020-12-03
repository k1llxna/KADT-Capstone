using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBall : MonoBehaviour
{
    public Vector3 force;
    Rigidbody rb;

    SphereCollider collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddRelativeForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //Explode
        //Stop movement
        rb.velocity = Vector3.zero;
        //Turn off collision
        collider.enabled = false;

        if (collision.transform.tag.Equals("Enemy"))
        {
            collision.gameObject.GetComponent<Monster>();
            //Monster.Explode(transform.position);
        }


        Destroy(gameObject, 1f);
    }
}
