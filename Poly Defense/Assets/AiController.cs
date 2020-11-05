using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{

    MonoGraph mGraph;
    Transform target;

    Kinematic body = new Kinematic();
    Arrive arrive = new Arrive();

    public float speed;
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        mGraph = FindObjectOfType<MonoGraph>();

        arrive.character = body;
        arrive.maxAcceleration = acceleration;
        arrive.maxSpeed = speed;
        arrive.targetRadius = 1;
        arrive.slowRadius = 1;

        body.rotSpeed = 10;
        body.position = transform.position;
    }

    public void StartPathfinding(Transform toTarget)
    {
        StopAllCoroutines();
        StartCoroutine(Pathfind(toTarget));
    }

    IEnumerator Pathfind(Transform target)
    {
        Graph<MonoNode> graph = mGraph.graph;
        Node<MonoNode> startNode = mGraph.findClosestNode(transform);
        Node<MonoNode> endNode = mGraph.findClosestNode(target);

        var camefrom = GraphSearch<MonoNode>.BFS(graph, startNode, endNode);

        List<Transform> waypointList = new List<Transform>();

        Node<MonoNode> currentNode = endNode;

        //Add the final target to the waypoint list
        waypointList.Add(target);

        //Add the rest going from finalDestination to startingPoint
        while (currentNode !=startNode)
        {
            waypointList.Add(currentNode.Value.transform);

            camefrom.TryGetValue(currentNode, out currentNode);
        }
               

        int currentWaypoint = waypointList.Count - 1;
        Transform targetDestination = waypointList[currentWaypoint];

        arrive.target = targetDestination;

        while (true)
        {

            //If it can see the final destination, just go straight there
            Ray ray = new Ray(transform.position + (Vector3.up * 2), (target.position - (transform.position + (Vector3.up * 2))).normalized * 100);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.point == target.position)
                {
                    currentWaypoint = 0;
                    targetDestination = waypointList[currentWaypoint];
                    arrive.target = targetDestination;

                    Debug.DrawLine(ray.origin, hit.point, Color.yellow, .05f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.blue, .05f);
                }
            }

            //If the target is near the waypoint, we can change waypoints
            if ((targetDestination.position - transform.position).magnitude < 1f)
            {
                //Unless we are at the end of the waypoint list
                if (currentWaypoint == 0)
                {
                    break;
                }

                currentWaypoint--;
                targetDestination = waypointList[currentWaypoint];
                arrive.target = targetDestination;
            }


            body.Update(arrive.getSteering(), Time.deltaTime);

            transform.position = body.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);


            yield return new WaitForEndOfFrame();
        }


    }



}
