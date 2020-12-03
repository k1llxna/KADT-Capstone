﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapToPlayer : MonoBehaviour
{
    Rigidbody rb;
    public float explosionForce;
    public float attractionForce;

    Vector3 offset = new Vector3(0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 force = (other.transform.position + offset - transform.position).normalized * explosionForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 force = (other.transform.position + offset - transform.position).normalized * attractionForce;
            rb.AddForce(force, ForceMode.Acceleration);

            if((other.transform.position + offset - transform.position).magnitude <= 1)
            {
                other.gameObject.GetComponent<Character>().GiveMoney(5);
                Destroy(gameObject);
            }
        }
    }

}
