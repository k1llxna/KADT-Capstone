using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
    public float maxHealth;
    float health;

    public int damage;
    public float speed;

    Animator animator;

    Static body = new Static();

    KinematicArrive seek = new KinematicArrive();

    GameObject target;

    public Slider healthBar;
    public Canvas UI;
    public TextMeshProUGUI healthText;

    public GameObject dropOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        body.position = transform.position;
        health = maxHealth;

        //target = FindObjectOfType<Structure>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        GetCloserTarget();

        if(!target)
        {
            FindTarget();
        }

        if (target)
        {
            Move();
        }

        UpdateHealthBar();

        body.position = transform.position;
    }

    protected virtual void Attack()
    {
        if(target.tag.Equals("Player"))
        {
            target.GetComponent<Character>().TakeDamage(damage);
        }
        else
        {
            target.GetComponent<Structure>().TakeDamage(damage);
        }
    }

    void Move()
    {
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
    }
    void GetCloserTarget()
    {
        //Get all structures
        Structure[] towers = FindObjectsOfType<Structure>();
            

        if (towers.Length > 0)
        {
            if (!target)
                target = towers[0].gameObject;

            //Check for closest structure
            foreach (Structure t in towers)
            {
                UnityEngine.Vector3 dis = t.gameObject.transform.position - transform.position;
                UnityEngine.Vector3 targetDis = target.transform.position - transform.position;

                if (dis.magnitude < targetDis.magnitude)
                {
                    target = t.gameObject;
                }
            }
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

    void UpdateHealthBar()
    {
        healthBar.value = health / maxHealth;
        healthText.SetText(health.ToString());
    }


    void Die()
    {
        //Make sure we have an item to drop
        if (dropOnDeath)
        {
            int drops = Random.Range(4, 20);
            
            drops = Mathf.RoundToInt(drops / 5);

            Debug.Log("Im dying and spawning: " + drops + " drops");

            Vector3 offset = new Vector3(0, 1, 0);

            for (int i = 0; i < drops; i++)
            {
                Instantiate(dropOnDeath, transform.position + offset, transform.rotation);
            }
        }
        else
        {
            Debug.Log("No Drops Enabled");
        }

        //Kill
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If we are near the player we want to target them
        if(other.tag.Equals("Player"))
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Upon a player leaving the attack range, we want to reset the target
        if (other.tag.Equals("Player"))
        {
            target = null;
        }
    }
}
