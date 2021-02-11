using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ServerMage : ServerPlayer
{
    public GameObject bullet;

    // Update is called once per frame

    [PunRPC]
    void Attack(Vector3 position, Quaternion rotation)
    {
        Vector3 offset = new Vector3(0, 1, 0);
        Instantiate(bullet, position + offset, rotation);
    }

    [PunRPC]
    void BuildTower(Vector3 position, int towerNum)
    {
        Instantiate(towers[towerNum], position, transform.rotation);
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
    void RPC_SetSpeed(float speed)
    {
        animator.SetFloat("Speed", Mathf.Abs(speed));
    }
    [PunRPC]
    void RPC_SetGrounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }
    [PunRPC]
    void RPC_Ability()
    {
        abilities[0].Use(this);
    }

}
