using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy")){
            other.GetComponent<Monster>().TakeDamage(damage);
        }

        Debug.Log(other);

        Destroy(gameObject, 0.1f);
    }
}
