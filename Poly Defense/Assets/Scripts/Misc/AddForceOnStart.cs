using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{

    public float force;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddRelativeForce(Vector3.forward * force);
    }
}
