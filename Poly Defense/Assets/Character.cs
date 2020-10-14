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
        switch(type)
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

        animator.SetFloat("Speed", Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", controller.isGrounded);
    }

    public void DealDamage(float damage)
    {
        health -= damage;
    }
}
