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
    public float health;

    public int damage;
    public float speed;
    public float acceleration;

    Animator animator;

    //Kinematic and method of movement
    Kinematic body = new Kinematic();
    Arrive arrive = new Arrive();

    NavMesh NavMesh;

    //Pathfinding Helpers
    public Transform target;
    Transform currentTarget;
    List<Transform> waypointList = new List<Transform>();
    int currentWaypoint = 0;

    //Should be in another script on the Canvas
    public Slider healthBar;
    public Canvas UI;
    public TextMeshProUGUI healthText;

    //Items
    public GameObject dropOnDeath;
    public GameObject[] powerups;

    public Rigidbody rb;
    public RagDollEffects ragdoll;

    //This allows use of RagDoll or other physics on the body without being constrained to the Kinematic, In the future kinematic should have it's own rigidbody features
    bool stable = true;
    bool attacking = false;
    public bool isGrounded = true;

    enum States {     
        IDLE,
        PATHFINDING,
        ATTACKING,            
    }

    States enemyState;

    // Start is called before the first frame update
    void Start()
    {
        NavMesh = FindObjectOfType<NavMesh>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(10, 10, true);

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

        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {

        switch (enemyState)
        {
            case States.IDLE:

                target = null;

                animator.SetBool("Running", false);
                body.velocity = Vector3.zero;

                if (FindTarget())
                    enemyState = States.PATHFINDING;

                break;

            case States.PATHFINDING:

                animator.SetBool("Running", true);
                //If we've lost a target or cant reach target go back to idle
                if (!target || !UpdatePathfinding())
                {
                    enemyState = States.IDLE;
                    break;
                }

                if ((transform.position - target.position).magnitude <= 1.5f)
                {
                    //Set enemy state to attacking
                    enemyState = States.ATTACKING;
                    break;
                }

                Move();

                ShowMovement();

                break;

            case States.ATTACKING:
                if (!attacking)
                {
                    StartCoroutine(StartAttacking());
                }
                if (!target)
                    enemyState = States.IDLE;
                break;
        }
    }

    protected virtual void Attack()
    {
        if (target.tag.Equals("Player"))
        {
            target.GetComponent<Character>().TakeDamage(damage);
        }
        else
        {
            target.GetComponent<Structure>().TakeDamage(damage);
        }
    }

    IEnumerator StartAttacking()
    {
        //Broken animation - to keepo enemy in place
        Vector3 position = transform.position;

        attacking = true;

        animator.SetBool("Running", false);
        animator.SetBool("Attacking", true);

        //Stop all movement
        body.velocity = Vector3.zero;

        while (target)
        {
            //As long as we are stable, keep enemy in place
            if(stable)
            {
                transform.position = position;
            }

            //Make sure we are still in range of the target
            //Upon killing the target, target will be instantly replaced, so dont keep attacking when change targets aswell - unless theyre right beside somehow
            if ((transform.position - target.position).magnitude <= 1.5f)
            {
                Attack();

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }

        attacking = false;

        animator.SetBool("Attacking", false);

        enemyState = States.IDLE;

        //UpdatePathfinding();
    }

    void ShowMovement()
    {
        for (int i = 0; i < waypointList.Count - 1; i++)
        {
            Debug.DrawLine(waypointList[i].position, waypointList[i + 1].position, Color.white);
        }
    }

    void Move()
    {
        //If we are at the target
       
        //At current target  - change waypoint
        if ((transform.position - currentTarget.position).magnitude <= 1f)
        {
            //We are right behind the target, we cant pathfind as this is the last node
            if (currentWaypoint == 1)
            {
                currentWaypoint--;
                currentTarget = waypointList[currentWaypoint];
                arrive.target = currentTarget;
            }
        }

        body.Update(arrive.GetSteering(), Time.deltaTime);
        transform.position = body.position;

        transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);

        animator.SetBool("Running", true);
    }

    bool FindTarget()
    {
        //Structure should include Player's Base
        Structure[] towers = FindObjectsOfType<Structure>();
        GameObject Base = GameObject.FindGameObjectWithTag("Base");

        if(!Base)
        {
            Debug.Log("Cant find a base");
            return false;
        }

        //If we can get to the goal position
        if (NavMesh.GetWaypoints(transform, Base.transform, out waypointList))
        {
            currentWaypoint = waypointList.Count - 1;
            currentTarget = waypointList[currentWaypoint];
            arrive.target = currentTarget;
            target = Base.transform;         
            Debug.Log("Attacking the base");
            return true;
        }
        else
        {
            //If not find closest possible tower blocking the path
            foreach (Structure tower in towers)
            {
                //Maybe add logic so we go to the closest one on the way to base
                //Right now it could possibly back track if there was a tower off to the side or behind that was closer

                //If we can navigate to this tower - do so
                if (NavMesh.GetWaypoints(transform, tower.transform, out waypointList))
                {
                    currentWaypoint = waypointList.Count - 1;
                    currentTarget = waypointList[currentWaypoint];
                    arrive.target = currentTarget;
                    target = tower.transform;
                    Debug.Log("Attacking the closest Tower");
                    return true;
                }

            }
        }

        return false;
    }

    bool UpdatePathfinding()
    {
        //We are already infront of the target, we dont need to update
        if(currentWaypoint <= 1)
        {
            return true;
        }

        //If we can get to the goal position
        if (NavMesh.GetWaypoints(transform, target.transform, out waypointList))
        {
            currentWaypoint = waypointList.Count - 1;
            currentTarget = waypointList[currentWaypoint];
            arrive.target = currentTarget;
            target = target.transform;
            return true;
        }

        return false;
    }

    public void TakeDamage(float damage, Vector3 from)
    {
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
        {
            Die(from);
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.value = health / maxHealth;
        healthText.SetText(Mathf.RoundToInt(health).ToString());
    }


    void Die(Vector3 from)
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

        if (powerups.Length > 0)
        {
            int chance = Random.Range(0, 20);

            if (chance > 15)
            {
                int powerupDrop = Random.Range(0, powerups.Length);

                Debug.Log("Im dying and spawning: " + powerups[powerupDrop] + " drops");

                Vector3 offset = new Vector3(0, 1, 0);

                    Instantiate(powerups[powerupDrop], transform.position + offset, transform.rotation);
            }                    
        }
        else
        {
            Debug.Log("No PowerUps Enabled");
        }

        Explode(from);

        //Kill
        Destroy(gameObject, 5f);
    }
    public void Explode(Vector3 position)
    {
        stable = false;
        isGrounded = false;

        //Turn to ragdoll physics

        animator.SetBool("Running", false);
        animator.SetBool("Attacking", false);
        attacking = false;

        animator.enabled = false;

        ragdoll.TurnOn();

        rb.useGravity = true;
        rb.AddExplosionForce(100, position, 100, 2, ForceMode.Impulse);


        if (health > 0)
        {
            StartCoroutine(Stabelize());
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If we are near the player we want to target them
        if (other.tag.Equals("Player"))
        {
            //Update pathfinding to player
            target = other.gameObject.transform;

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

    private void ResetKinematics()
    {
        body = new Kinematic();
        arrive = new Arrive();

        body.rotSpeed = 10;

        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        body.position = transform.position;

        arrive.character = body;
        arrive.maxAcceleration = acceleration;
        arrive.maxSpeed = speed;
        arrive.targetRadius = 1;
        arrive.slowRadius = 1;
    }

    IEnumerator Stabelize()
    {

        yield return new WaitForSeconds(3f);

        ragdoll.TurnOff();

        animator.enabled = true;
        stable = true;

        ResetKinematics();
        animator.SetBool("Running", true);
    }
}
