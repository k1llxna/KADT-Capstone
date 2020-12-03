using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMage : ServerPlayer
{
    public GameObject bullet;

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Move();

            if (!building)
            {
                if (Input.GetKeyDown("k"))
                    StartCoroutine("Building");

                if (Input.GetMouseButtonDown(0))
                    photonView.RPC("Attack", RpcTarget.All, transform.position, Camera.main.transform.rotation);

                //Get information of targeted object with raycast
                Target();
            }
        }

    }

    [PunRPC]
    void Attack(Vector3 position, Quaternion rotation)
    {
        Vector3 offset = new Vector3(0, 1, 0);
        Instantiate(bullet, position + offset, rotation);
    }

    [PunRPC]
    void BuildTower(Vector3 position)
    {
        Instantiate(towers[0], position, transform.rotation);
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

}
