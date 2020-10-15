using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    public float gravity;
    bool isGrounded;

    public float health;


    Vector3 moveDirection = Vector3.zero;



    public GameObject bullet;

    enum ControllerType {  SimpleMove, Move };
    [SerializeField] ControllerType type;

    void Awake()
    {
        
    }

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
        Move();


        if (Input.GetMouseButtonDown(0))
            Attack();

        //Get information of targeted object with raycast
        Target();

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", controller.isGrounded);
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

        

        float yLerp = Mathf.LerpAngle(transform.rotation.y, Camera.main.GetComponentInParent<Transform>().rotation.y, 0.5f);

        Quaternion newRotation = new Quaternion(transform.rotation.x, yLerp, transform.rotation.z, transform.rotation.w);

        transform.rotation = newRotation;
    }

    void Attack()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        Instantiate(bullet, transform.position + offset, Camera.main.transform.rotation);
    }

    void Target()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 1, 0);

        if (Physics.Raycast(transform.position + offset, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag.Equals("Enemy"))
            {
                Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            }
        }
        else
        {
            Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, Color.yellow);
        }
    }

    //Take damage based on damage that was dealt
    public void DealDamage(float damage)
    {
        health -= damage;
    }
}
