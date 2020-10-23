using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{

    public Transform target;
    public Transform[] waypoints;
    public float speed;
    public float acceleration;

    public Animator animator;

    Seek seek = new Seek();

    Arrive arrive = new Arrive();
    ObstacleAvoidance avoid = new ObstacleAvoidance();

    Kinematic body = new Kinematic();
    Kinematic bodyTarget = new Kinematic();
    Align align = new Align();

    public bool Arrive;
    public bool pathfind;
    bool isPathfinding;

    int currentWaypoint = 0;

    private void Start()
    {
        seek.character = body;
        seek.maxAcceleration = speed;

        arrive.character = body;
        arrive.maxAcceleration = acceleration;
        arrive.maxSpeed = speed;
        arrive.targetRadius = 1;
        arrive.slowRadius = 1;

        avoid.character = body;
        avoid.maxAcceleration = acceleration;
        avoid.maxSpeed = speed;
        avoid.targetRadius = 1;
        avoid.slowRadius = 1;
        avoid.avoidDistance = 1;
        avoid.lookAhead = 1;

        body.rotSpeed = 10;

        body.position = transform.position;

        StartCoroutine("Pathfind");
        
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(body.position, body.velocity, Color.blue);
    }

    /*
    void UpdatePosition()
    {
        if(!pathfind)
        {
            if (Arrive)
            {
                body.UpdateObject(arrive.getSteering(), Time.deltaTime);
            }
            else
            {
                body2.Update(align.getSteering(), Time.deltaTime);
            }

            transform.position = body2.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, body2.orientation * Mathf.Rad2Deg, transform.rotation.z);
        }
      
    }
    */


    IEnumerator Pathfind()
    {
        isPathfinding = true;

        target = waypoints[currentWaypoint];

        arrive.target = target;
        avoid.target = target;

        while(isPathfinding)
        {
            //If the target is near the waypoint, we can change waypoints
            if((target.position - transform.position).magnitude < 1f)
            {
                //Unless we are at the end of the waypoint list
                if (currentWaypoint == waypoints.Length - 1)
                {
                    isPathfinding = false;
                    break;
                }

                currentWaypoint++;
                target = waypoints[currentWaypoint];
                arrive.target = target;
                avoid.target = target;
            }
            else
            {
                body.Update(avoid.getSteering(), Time.deltaTime);

                transform.position = body.position;
                transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);
            }

            
            yield return new WaitForEndOfFrame();
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Structure"))
        {
            animator.SetBool("Attacking", true);
        }
    }

}
