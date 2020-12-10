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
    //Attributes
    public float maxHealth;
    float health;
    
    public int damage;
    public float speed;
    public float acceleration;

    Animator animator;

    //Kinematic and method of movement
    Kinematic body = new Kinematic();
    Arrive arrive = new Arrive();

    NavMesh NavMesh;

    //Pathfinding Helpers
    Transform target;
    Transform currentTarget;
    List<Transform> waypointList = new List<Transform>();
    int currentWaypoint = 0;

    //Should be in another script on the Canvas
    public Slider healthBar;
    public Canvas UI;
    public TextMeshProUGUI healthText;

    //Items
    public GameObject dropOnDeath;

    public Rigidbody rb;

    //This allows use of RagDoll or other physics on the body without being constrained to the Kinematic, In the future kinematic should have it's own rigidbody features
    bool stable = true;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        NavMesh = FindObjectOfType<NavMesh>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //Make sure enemy is full health
        health = maxHealth;

        //Setup Kinematics
        body = new Kinematic();
        arrive = new Arrive();

        body.rotSpeed = 10;
        body.position = transform.position;        

        arrive.character = body;
        arrive.maxAcceleration = acceleration;
        arrive.maxSpeed = speed;
        arrive.targetRadius = 1;
        arrive.slowRadius = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Prevent doing nothing/errors
        if(!target)
        {
            FindTarget();
        }

        if (target)
        {
            Move();
        }
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
        if ((transform.position - target.position).magnitude <= 1)
        {
            //This can be an Ienumurator and we can call a bool that stops movement
            Attack();
            animator.SetBool("Running", false);
            animator.SetTrigger("Attack");
        }
        else if ((transform.position - currentTarget.position).magnitude <= 1f)
        {
            currentWaypoint--;
            currentTarget = waypointList[currentWaypoint];
            arrive.target = currentTarget;
        }

        body.Update(arrive.GetSteering(), Time.deltaTime);
        transform.position = body.position;

        transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);

        animator.SetBool("Running", true);
    }

    void FindTarget()
    {
        //Structure should include Player's Base
        Structure[] towers = FindObjectsOfType<Structure>();
        Character[] characters = FindObjectsOfType<Character>();
        GameObject Base = GameObject.FindGameObjectWithTag("Base");

        //If we can get to the goal position
        if(NavMesh.GetWaypoints(transform, Base.transform, out waypointList))
        {
            currentWaypoint = waypointList.Count - 1;
            currentTarget = waypointList[currentWaypoint];
            arrive.target = currentTarget;
            target = Base.transform;
        }
        else
        {
            //If not find closest possible tower blocking the path
            foreach(Structure tower in towers)
            {
                //Maybe add logic so we go to the closest one on the way to base
                //Right now it could possibly back track if there was a tower off to the side or behind that was closer

                //If we can navigate to this tower - do so
                if(NavMesh.GetWaypoints(transform, tower.transform, out waypointList))
                {
                    currentWaypoint = waypointList.Count - 1;
                    currentTarget = waypointList[currentWaypoint];
                    arrive.target = currentTarget;
                    target = tower.transform;
                    break;
                }

            }
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
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
        Destroy(gameObject, 2f);
    }
    public void Explode(Vector3 position, int damage)
    {
        stable = false;
        rb.AddExplosionForce(20, position, 100, 2, ForceMode.Impulse);
        TakeDamage(damage);

        //StartCoroutine(Stabelize());
    }

    private void OnTriggerEnter(Collider other)
    {
        //If we are near the player we want to target them
        if(other.tag.Equals("Player"))
        {
            target = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Upon a player leaving the attack range, we want to reset the target
        if (other.tag.Equals("Player"))
        {
            FindTarget();
        }
    }

    IEnumerator Stabelize()
    {
        

        yield return new WaitForSeconds(1f);


    }
}
