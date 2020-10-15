using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Monster : MonoBehaviour
{
    public float health;
    public float damage;
    public float speed;

    Animator animator;

    Static body = new Static();

    KinematicArrive seek = new KinematicArrive();

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        body.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!target)
        {
            FindTarget();
        }

        if ((transform.position - target.transform.position).magnitude <= 1)
        {
            Attack();
            animator.SetBool("Running", false);
            animator.SetTrigger("Attack");
        }
        else
        {
            //BLAH BLAH DO PATHFINDING TO TARGET HERE;
            seek.target = target.transform.position;
            seek.maxSpeed = speed;
            seek.character = body;

            body.UpdateObject(seek.getSteering(), Time.deltaTime);
            transform.position = body.position;

            animator.SetBool("Running", true);
        }
    }


    protected virtual void Attack()
    {
        if(target.tag.Equals("Player"))
        {
            target.GetComponent<Character>().DealDamage(damage);
        }
        else
        {
            target.GetComponent<Structure>().DealDamage(damage);
        }
    }

    void FindTarget()
    {
        //Structure should include Player's Base
        Structure[] towers = FindObjectsOfType<Structure>();
        Character[] characters = FindObjectsOfType<Character>();

        if (towers.Length > 0)
        {

            Structure closestStruct = towers[0];
            float distance = 9999999;

            foreach (Structure t in towers)
            {
                UnityEngine.Vector3 dis = t.gameObject.transform.position - transform.position;

                if (dis.magnitude < distance)
                {
                    closestStruct = t;
                    distance = dis.magnitude;
                }
            }

            target = closestStruct.gameObject;
        }
        else if (characters.Length > 0)
        {
            Character closestCharacter = characters[0];
            float distance = 9999999;

            foreach (Character c in characters)
            {
                UnityEngine.Vector3 dis = c.gameObject.transform.position - transform.position;

                if (dis.magnitude < distance)
                {
                    closestCharacter = c;
                    distance = dis.magnitude;
                }
            }

            target = closestCharacter.gameObject;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
