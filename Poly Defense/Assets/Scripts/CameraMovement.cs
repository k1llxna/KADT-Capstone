using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.localPosition = transform.position + offset;
        //Camera.main.transform.rotation = transform.rotation;
    }
}
