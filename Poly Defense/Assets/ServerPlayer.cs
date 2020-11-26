﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ServerPlayer : MonoBehaviourPun
{
    protected CharacterController controller;    
    protected Animator animator;

    //public Transform playerTransform;
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    public float gravity;
    protected bool isGrounded;
    

    public float health;

    protected Vector3 moveDirection = Vector3.zero;

    public GameObject[] towers;
    protected bool building;

    protected enum ControllerType { SimpleMove, Move };
    [SerializeField] protected ControllerType type;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

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

    protected IEnumerator Building()
    {
        LayerMask ground = 1 << 9;

        bool hasBuilt = false;
        building = true;

        //This would be a tempTower array, and will instantiate the real tower later
        GameObject tower = Instantiate(towers[0], new Vector3(100,100,100), transform.rotation);

        RaycastHit hit;
        Vector3 offset = new Vector3(0, 1, 0);
        Vector3 towerOffset = new Vector3(0, 0.5f, 0);

        while (!hasBuilt)
        {
            Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 5, Color.red);

            if (Physics.Raycast(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 5, ground))
            {
                tower.transform.position = hit.point + towerOffset;
                tower.transform.rotation = transform.rotation;
            }

            if (Input.GetMouseButton(0))
            {
                Debug.Log("finished building");
                Destroy(tower);

                photonView.RPC("BuildTower", RpcTarget.AllBuffered, tower.transform.position);                
                hasBuilt = true;
            }

            yield return new WaitForEndOfFrame();
        }

        //While(!hasTurned)
        //allow option for fine rotatating of the newly built tower

        building = false;
    }

    protected void Move()
    {
        switch (type)
        {
            case ControllerType.SimpleMove:
                controller.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));
                break;

            case ControllerType.Move:
                if (isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);

                    moveDirection *= speed;

                    if (Input.GetButtonDown("Jump"))
                        moveDirection.y = jumpSpeed;

                    /*
                    if(moveDirection.magnitude > 0.1)
                    {
                        photonView.RPC("RPC_SetSpeed", RpcTarget.All, moveDirection.z);
                    }
                    */
                }

                moveDirection.y -= gravity * Time.deltaTime;

                controller.Move(moveDirection * Time.deltaTime);

                photonView.RPC("RPC_SetSpeed", RpcTarget.All, moveDirection.x);
                photonView.RPC("RPC_SetPosition", RpcTarget.All, transform.position);

                if (controller.isGrounded)
                    isGrounded = true;
                
                break;
        }



        float yLerp = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 0.05f);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.x, yLerp, transform.rotation.z);
        transform.rotation = newRotation;

        photonView.RPC("RPC_SetRotation", RpcTarget.All, transform.rotation);

    }

    protected void Target()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 1, 0);

        if (Physics.Raycast(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag.Equals("Enemy"))
            {
                UIHandler uihandler = hit.transform.gameObject.GetComponent<UIHandler>();
                uihandler.StopAllCoroutines();
                uihandler.StartCoroutine("ShowUI");

                //Keeps HealthBar Roughly the same size throughout
                float size = (transform.position - hit.transform.position).magnitude / 100;
                if (size < 0.08)
                    size = 0.08f;

                uihandler.slider.transform.localScale = new Vector3(size, size, size);

                Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            }
        }
        else
        {
            Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, Color.yellow);
        }
    }

    //Take damage based on damage that was dealt
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    [PunRPC]
    void Attack(Vector3 position, Quaternion rotation)
    {
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
    void BuildTower(Vector3 position)
    {
        Instantiate(towers[0], position, transform.rotation);
    }
}
