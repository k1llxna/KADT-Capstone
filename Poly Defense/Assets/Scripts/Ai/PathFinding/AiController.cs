using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{

    MonoGraph mGraph;

    Kinematic body = new Kinematic();
    Arrive arrive = new Arrive();

    public float speed;
    public float acceleration;
    public Transform target;
    Transform currentTarget;
    public List<Transform> waypointList = new List<Transform>();
    int currentWaypoint = 0;

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

        //On start begin pathfinding to base
        target = GameObject.FindGameObjectWithTag("Base").transform;
        UpdatePathfinding();

        StartPathfinding();
    }

    private void Update()
    {

    }

    public void StartPathfinding()
    { 
        //Starts Pathfinding Enumerator
        StopAllCoroutines();
        StartCoroutine(Pathfind());
    }

    IEnumerator Pathfind()
    {        
        currentTarget = waypointList[currentWaypoint];

        while (true)
        {
            //If it can see the final destination, just go straight there
            Ray ray = new Ray(transform.position + (Vector3.up), (target.position - (transform.position + (Vector3.up * 1))).normalized * 100);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform == target)
                {
                    currentWaypoint = 0;
                    currentTarget = waypointList[currentWaypoint];
                    arrive.target = currentTarget;

                    Debug.DrawLine(ray.origin, hit.point, Color.yellow, .05f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.blue, .05f);
                }
            }

            //If the target is near the waypoint, we can change waypoints
            if ((currentTarget.position - transform.position).magnitude < 1f)
            {
                //Unless we are at the end of the waypoint list
                if (currentWaypoint == 0)// || (target.position - transform.position).magnitude < 0.2f)
                {
                    //We are at the end of the waypoint list
                }
                else
                {
                    currentWaypoint--;
                    currentTarget = waypointList[currentWaypoint];
                    arrive.target = currentTarget;
                }
            }

            //Update body and transform
            body.Update(arrive.getSteering(), Time.deltaTime);

            transform.position = body.position;
            transform.rotation = Quaternion.Euler(transform.rotation.x, body.orientation * Mathf.Rad2Deg, transform.rotation.z);

            yield return new WaitForEndOfFrame();
        }
    }

    public void UpdatePathfinding()
    {
        waypointList.Clear();

        //Generate Waypoint list
        Graph<MonoNode> graph = mGraph.graph;

        //Get closest node to target
        Node<MonoNode> endNode = mGraph.FindClosestNode(target);

        //Get best node to start at
        Node<MonoNode> startNode = mGraph.FindClosestNode(transform);
        
        var camefrom = GraphSearch<MonoNode>.Dijkstra(graph, startNode, endNode);

        Node<MonoNode> currentNode = endNode;

        //Add the final target to the waypoint list
        waypointList.Add(target);

        //Add the rest going from finalDestination to startingPoint
        while (currentNode != startNode)
        {
            waypointList.Add(currentNode.Value.transform);

            camefrom.TryGetValue(currentNode, out currentNode);
        }

        //Add starting node -- 
        waypointList.Add(startNode.Value.transform);

        //On pathfinding reset, change waypoint to last waypoint *closest to zombie
        currentWaypoint = waypointList.Count - 1;
        currentTarget = waypointList[currentWaypoint];
        arrive.target = currentTarget;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;
            UpdatePathfinding();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Base").transform;
            UpdatePathfinding();
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.CompareTag("Base"))
            Destroy(gameObject);
    }
}
