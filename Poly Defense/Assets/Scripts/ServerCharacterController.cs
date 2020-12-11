using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCharacterController : MonoBehaviourPun
{


    private void Update()
    {
        photonView.RPC("RPC_SetPosition", RpcTarget.All, transform.position);
        photonView.RPC("RPC_SetRotation", RpcTarget.All, transform.rotation);
    }    
        

    [PunRPC]
    void RPC_SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    [PunRPC]
    void RPC_SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    [PunRPC]
    protected virtual void Attack()
    {
    }
};
