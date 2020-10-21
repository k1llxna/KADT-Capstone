using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{

    Static body = new Static();
    public Vector3 target;
    public Vector3[] waypoints;
    public float speed;

    KinematicSeek seek = new KinematicSeek();

    KinematicArrive arrive = new KinematicArrive();
    

    public bool Arrive;
    public bool pathfind;
    bool isPathfinding;

    int currentWaypoint = 0;

    private void Start()
    {
        seek.character = body;
        seek.maxSpeed = speed;
        seek.target = target;

        arrive.character = body;
        arrive.maxSpeed = speed;
        arrive.target = target;
        arrive.timeToTarget = 0.1f;
        arrive.radius = 1f;

        body.position = transform.position;

        StartCoroutine("Pathfind");

    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();       
    }

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
                body.UpdateObject(seek.getSteering(), Time.deltaTime);
            }

            transform.position = body.position;
        }
      
    }


    IEnumerator Pathfind()
    {
        isPathfinding = true;

        target = waypoints[currentWaypoint];

        arrive.target = target;

        while(isPathfinding)
        {
            //If the target is near the waypoint, we can change waypoints
            if((target - transform.position).magnitude < 1f)
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
            }

            body.UpdateObject(arrive.getSteering(), Time.deltaTime);
            
            transform.position = body.position;
            print(body.orientation);
            transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);

            yield return new WaitForEndOfFrame();
        }


    }

}
