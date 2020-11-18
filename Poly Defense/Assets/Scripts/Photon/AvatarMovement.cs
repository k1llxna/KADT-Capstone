using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviourPun
{

    CharacterController controller;
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    public float gravity;


    Vector3 moveDirection = Vector3.zero;

    enum ControllerType { SimpleMove, Move };
    [SerializeField] ControllerType type;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Physics.IgnoreLayerCollision(8, 8, true);

        if (speed <= 0)
        {
            speed = 6.0f;
            Debug.Log("Speed not set on " + name + " defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;
            Debug.Log("JumpSpeed not set on " + name + " defaulting to " + jumpSpeed);
        }

        if (rotationSpeed <= 0)
        {
            rotationSpeed = 10.0f;
            Debug.Log("RotationSpeed not set on " + name + " defaulting to " + rotationSpeed);
        }

        if (gravity <= 0)
        {
            gravity = 9.81f;
            Debug.Log("Gravity not set on " + name + " defaulting to " + gravity);
        }

        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        else
        {
            Move();
            photonView.RPC("SetPosition", RpcTarget.All, transform.position);
            photonView.RPC("SetRotation", RpcTarget.All, transform.rotation);
        }
    }


    private void Move()
    {
        switch (type)
        {
            case ControllerType.SimpleMove:
                controller.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));
                break;

            case ControllerType.Move:
                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);

                    moveDirection *= speed;

                    if (Input.GetButtonDown("Jump"))
                        moveDirection.y = jumpSpeed;
                }

                moveDirection.y -= gravity * Time.deltaTime;

                controller.Move(moveDirection * Time.deltaTime);
                break;
        }



        float yLerp = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 0.05f);

        Quaternion newRotation = Quaternion.Euler(transform.rotation.x, yLerp, transform.rotation.z);

        transform.rotation = newRotation;

    }

    [PunRPC]
    void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    [PunRPC]
    void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
