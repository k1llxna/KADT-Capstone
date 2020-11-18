using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviourPun
{
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        //Stick camera to local avatar
        if(photonView.IsMine)
        {
            Camera.main.transform.position = transform.position + offset;
            Camera.main.transform.rotation = transform.rotation;
        }
    }
}
