using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationScript : MonoBehaviour
{

    public Vector3 rotation;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
        //transform.rotation *= new Quaternion(rotation.x, rotation.y, rotation.z, 0);
    }
}
