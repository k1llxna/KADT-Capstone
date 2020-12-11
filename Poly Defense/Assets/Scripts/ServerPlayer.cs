using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ServerPlayer : Character
{

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Move();

            if (!building)
            {

                if (Input.GetKeyDown("1"))
                    StartCoroutine(Building(0));
                else if (Input.GetKeyDown("2"))
                    StartCoroutine(Building(1));
                else if (Input.GetKeyDown("3"))
                    StartCoroutine(Building(2));
                else if (Input.GetKeyDown("4"))
                    StartCoroutine(Building(3));


                else if (Input.GetKeyDown("5"))
                    abilities[0].Use(this);
                else if (Input.GetKeyDown("6"))
                    abilities[0].Use(this);
                else if (Input.GetKeyDown("7"))
                    abilities[0].Use(this);
                else if (Input.GetKeyDown("8"))
                    abilities[0].Use(this);

                if (Input.GetMouseButtonDown(0))
                    photonView.RPC("Attack", RpcTarget.All, transform.position, Camera.main.transform.rotation);

                //Get information of targeted object with raycast
                Target();
            }

            if (controller.isGrounded)
            {
                isGrounded = true;
                photonView.RPC("RPC_SetGrounded", RpcTarget.All, true);
            }


            UpdateMana();
        }
    }

    protected override void Move()
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
                    {
                        moveDirection.y = jumpSpeed;
                        isGrounded = false;
                        photonView.RPC("RPC_SetGrounded", RpcTarget.All, false);
                    }

                }
                else
                {
                    float tempY = moveDirection.y;

                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);

                    moveDirection *= speed;

                    moveDirection = new Vector3(moveDirection.x, tempY, moveDirection.z);

                    moveDirection.y -= gravity * Time.deltaTime;
                }


                controller.Move(moveDirection * Time.deltaTime);

                photonView.RPC("RPC_SetSpeed", RpcTarget.All, moveDirection.x + moveDirection.z);

                break;
        }



        float yLerp = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 0.05f);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.x, yLerp, transform.rotation.z);
        transform.rotation = newRotation;

        photonView.RPC("RPC_SetRotation", RpcTarget.All, transform.rotation);

    }

    protected override void Target()
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

            if (hit.transform.tag.Equals("Tower"))
            {
                TowerUIController uihandler = hit.transform.gameObject.GetComponent<TowerUIController>();
                OTower tower = hit.transform.gameObject.GetComponent<OTower>();

                if (Mathf.Abs((transform.position - hit.transform.position).magnitude) <= 10)
                {
                    uihandler.StopAllCoroutines();
                    uihandler.StartCoroutine("ShowUI");

                    //Keeps HealthBar Roughly the same size throughout
                    float size = (transform.position - hit.transform.position).magnitude / 100;
                    if (size < 0.08)
                        size = 0.08f;

                    uihandler.panel.transform.localScale = new Vector3(size, size, size);

                    Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                }

                if (tower.photonView.IsMine)
                {
                    if (Input.GetKeyDown("g"))
                    {
                        StartCoroutine(UpgradeBuilding(tower, uihandler));
                    }

                    else if (Input.GetKeyDown("e"))
                    {
                        StartCoroutine(RepairBuilding(tower));
                    }

                    else if (Input.GetKeyDown("v"))
                    {
                        StartCoroutine(SellBuilding(tower, uihandler));
                    }
                }

            }
        }
        else
        {
            Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, Color.yellow);
        }
    }

    IEnumerator Building(int towerNum)
    {
        OTower tower = towers[towerNum].GetComponent<OTower>();

        //Break if we dont have enough money
        if (money < tower.cost)
        {
            Debug.Log("Dont have enough mana to build tower");
            //Play error sound
        }
        else
        {
            //We can only raycast to the ground
            LayerMask ground = 1 << 9;

            bool hasBuilt = false;
            building = true;

            //This would be a tempTower array, and will instantiate the real tower later
            GameObject towerObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BArricade"), new Vector3(100, 100, 100), transform.rotation);

            RaycastHit hit;
            Vector3 offset = new Vector3(0, 1, 0);
            Vector3 towerOffset = new Vector3(0, 0.5f, 0);

            while (!hasBuilt)
            {
                Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 5, Color.red);

                if (Physics.Raycast(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 5, ground))
                {
                    towerObject.transform.position = hit.point + towerOffset;
                    towerObject.transform.rotation = transform.rotation;
                }

                if (Input.GetMouseButton(0))
                {
                    print("finished building");
                    money -= tower.cost;
                    hasBuilt = true;
                }

                yield return new WaitForEndOfFrame();
            }

            //While(!hasTurned)
            //allow option for fine rotatating of the newly built tower

            building = false;
        }
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
