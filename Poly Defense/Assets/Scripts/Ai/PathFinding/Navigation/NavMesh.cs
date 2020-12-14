using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    public MonoNode[] nodes;

    public Graph<MonoNode> graph;
    public NodeList<MonoNode> nodeSet = new NodeList<MonoNode>();

    public Color neighbourColor;

    void Start()
    {
        nodes = FindObjectsOfType<MonoNode>();

        //Populate NodeSet
        foreach (MonoNode node in nodes)
        {
            GraphNode<MonoNode> newNode = new GraphNode<MonoNode>(node);
            newNode.Value = node;
            nodeSet.Add(newNode);
        }

        //Create Graph
        graph = new Graph<MonoNode>(nodeSet);

        //Set GraphNode Neighbours
        foreach (GraphNode<MonoNode> gNode in nodeSet)
        {
            foreach (GraphNode<MonoNode> mNode in gNode.Neighbours)
            {
                //Change this so not grabbing from value, GraphNode should have a toCost and fromCost
                float cost = (mNode.Value.cost + gNode.Value.cost);

                graph.AddDirectedEdge(gNode, mNode, Mathf.RoundToInt(cost));
            }
        }

        UpdateGraph();
    }

    private void Update()
    {
        foreach (Node<MonoNode> node in nodeSet)
        {
            if (node.Value.isEnabled)
            {
                foreach (Node<MonoNode> neighbourNode in graph.Neighbours(node))
                {
                    Debug.DrawLine(node.Value.transform.position, neighbourNode.Value.transform.position, neighbourColor);
                }
            }
        }
    }

    //ResetGraph
    public void UpdateGraph()
    {
        graph.Reset();

        graph = new Graph<MonoNode>();

        //Create Graph
        graph = new Graph<MonoNode>(nodeSet);


        //Populate NodeSet
        foreach (MonoNode node in nodes)
        {
            GraphNode<MonoNode> newNode = new GraphNode<MonoNode>(node);
            newNode.Value = node;
            nodeSet.Add(newNode);
        }

        //Set GraphNode Neighbours
        foreach (GraphNode<MonoNode> gNode in nodeSet)
        {
            if (gNode.Value.isEnabled)
            {
                foreach (MonoNode mNode in gNode.Value.TrueNeighbours)
                {

                    GraphNode<MonoNode> neghbouringNode = (GraphNode<MonoNode>)nodeSet.FindByValue(mNode);

                    float cost = (mNode.cost + gNode.Value.cost);

                    graph.AddDirectedEdge(gNode, neghbouringNode, Mathf.RoundToInt(cost));

                }
            }
        }

    }

    //Return the absolute closest node
    public Node<MonoNode> FindClosestNode(Transform transform)
    {
        float distance = float.MaxValue;
        Node<MonoNode> closestNode = nodeSet[0];

        foreach (Node<MonoNode> node in nodeSet)
        {
            if (node.Value.isEnabled)
            {
                if (Mathf.Abs((node.Value.transform.position - transform.position).magnitude) < distance)
                {
                    closestNode = node;
                    distance = Mathf.Abs((node.Value.transform.position - transform.position).magnitude);
                }
            }
        }

        Debug.DrawLine(transform.position, closestNode.Value.transform.position, Color.red, 1f);

        return closestNode;
    }

    //Return the node that is fastest to goal
    public Node<MonoNode> FindBestNode(Transform transform, Node<MonoNode> goal)
    {
        List<Node<MonoNode>> seeableNodes = new List<Node<MonoNode>>();

        Node<MonoNode> closestNode = FindClosestNode(transform);

        foreach (Node<MonoNode> node in closestNode.Neighbours)
        {
            seeableNodes.Add(node);
        }

        float smallestCost = float.MaxValue;
        Node<MonoNode> bestNode = seeableNodes[0];

        //Now we had a list of seeable Nodes -- time to find the best node out of all of them
        foreach (Node<MonoNode> seeableNode in seeableNodes)
        {
            if (seeableNode.Value.isEnabled)
            {
                var camefrom = GraphSearch.ASearch(graph, seeableNode, goal);
                Node<MonoNode> currentNode = goal;

                //If we cant get to the this node
                if (!camefrom.ContainsKey(goal))
                    break;


                float totalCost = 0;

                while (currentNode != seeableNode)
                {
                    if (!currentNode.Value.isEnabled)
                    {
                        totalCost = float.MaxValue;
                        break;
                    }

                    totalCost += (currentNode.Value.cost + Heuristic(currentNode.Value.transform, seeableNode.Value.transform));

                    camefrom.TryGetValue(currentNode, out currentNode);
                }

                if (totalCost < smallestCost)
                {
                    bestNode = seeableNode;
                    smallestCost = totalCost;
                }
            }
        }

        Debug.DrawLine(transform.position, bestNode.Value.transform.position, Color.green, 1f);
        return bestNode;
    }

    public bool GetWaypoints(Transform start, Transform goal, out List<Transform> waypointList)
    {
        //List of waypoints - we return this
        List<Transform> waypoints = new List<Transform>();

        //Get To/From Nodes on Graph
        Node<MonoNode> goalNode = FindClosestNode(goal);
        Node<MonoNode> startNode = FindBestNode(start, goalNode);

        //Get the Dictionary of Searched Nodes
        var camefrom = GraphSearch.ASearch(graph, startNode, goalNode);

        //Early exit
        if (!camefrom.ContainsKey(goalNode))
        {
            //We did not complete a path to the startNode
            //Therefore we cannot navigate to this goal
            waypointList = waypoints;
            return false;
        }

        //Start from the end of the list
        Node<MonoNode> currentNode = goalNode;
        waypoints.Add(goal);

        //Add the rest going from finalDestination to startingPoint
        while (currentNode != startNode)
        {
            waypoints.Add(currentNode.Value.transform);
            camefrom.TryGetValue(currentNode, out currentNode);
        }

        //Add starting node -- 
        waypoints.Add(startNode.Value.transform);

        waypointList = waypoints;
        return true;

    }

    public void DisableBounds(Bounds bounds)
    {
        foreach (Node<MonoNode> node in nodeSet)
        {
            if (bounds.Contains(node.Value.transform.position))
            {
                node.Value.Disable();
            }
        }

        UpdateGraph();
    }

    public void EnableBounds(Bounds bounds)
    {
        foreach (Node<MonoNode> node in nodeSet)
        {
            if (bounds.Contains(node.Value.transform.position))
            {
                node.Value.Enable();
            }
        }

        UpdateGraph();
    }

    public void DisableNode(Node<MonoNode> node)
    {
        node.Value.Disable();
        UpdateGraph();
    }

    public void EnableNode(Node<MonoNode> node)
    {
        node.Value.Enable();
        UpdateGraph();
    }

    float Heuristic(Transform start, Transform finish)
    {
        return (start.position - finish.position).magnitude;
    }
}
