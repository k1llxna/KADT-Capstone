using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThing : MonoBehaviour
{

    public Transform target;
    Vector3 offset = new Vector3(0, 1, 0);


     void Update()
    {
        transform.position = target.position + offset;
    }
}
