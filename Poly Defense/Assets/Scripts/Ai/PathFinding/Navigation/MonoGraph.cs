using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGraph : MonoBehaviour
{
    public MonoNode[] nodes;

    public Graph<MonoNode> graph;
    public NodeList<MonoNode> nodeSet = new NodeList<MonoNode>();

    public Color neighbourColor;

    void Awake()
    {
        nodes = FindObjectsOfType<MonoNode>();

        //Populate NodeSet
        foreach(MonoNode node in nodes)
        {
            GraphNode<MonoNode> newNode = new GraphNode<MonoNode>(node);
            newNode.Value = node;
            nodeSet.Add(newNode);
        }

        //Create Graph
        graph = new Graph<MonoNode>(nodeSet);

        //Set GraphNode Neighbours
        foreach(GraphNode<MonoNode> gNode in nodeSet)
        {
            foreach(MonoNode mNode in gNode.Value.Neighbours)
            {
                GraphNode<MonoNode> neghbouringNode = (GraphNode<MonoNode>)nodeSet.FindByValue(mNode);

                float cost = (mNode.cost + gNode.Value.cost);

                graph.AddDirectedEdge(gNode, neghbouringNode, Mathf.RoundToInt(cost));
            }
        }
    }

    private void Update()
    {
        foreach(Node<MonoNode> node in nodeSet)
        {
            foreach(Node<MonoNode> neighbourNode in graph.Neighbours(node))
            {
                Debug.DrawLine(node.Value.transform.position, neighbourNode.Value.transform.position, neighbourColor);
            }
        }
    }

    //Return the absolute closest node
    public Node<MonoNode> FindClosestNode(Transform transform)
    {
        float distance = float.MaxValue;
        Node<MonoNode> closestNode = nodeSet[0];

        foreach(Node<MonoNode> node in nodeSet)
        {
            if (Mathf.Abs((node.Value.transform.position - transform.position).magnitude) < distance)
            {
                
                //If the node can see the transform it is a valid node -- Object cannot use a close node that is behind a wall
                Ray ray = new Ray(node.Value.transform.position + Vector3.up, (transform.position - (node.Value.transform.position + Vector3.up)).normalized);
                RaycastHit hit;

                if(Physics.Raycast(ray.origin, ray.direction * 100, out hit, 100f))
                {
                    if(hit.transform == transform)
                    {
                        closestNode = node;
                        distance = Mathf.Abs((node.Value.transform.position - transform.position).magnitude);
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 1f);
                        
                    }

                    print(hit.transform);
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 1f);
                }
            }
        }
        return closestNode;
    }

    //Return the node that is fastest to goal
    public Node<MonoNode> FindBestNode(Transform transform, Node<MonoNode> goal)
    {
        List<Node<MonoNode>> seeableNodes = new List<Node<MonoNode>>();

        foreach (Node<MonoNode> node in nodeSet)
        {
            //If the node can see the transform it is a valid node -- Object cannot use a close node that is behind a wall
            //Add that node to the seeable nodes list
            Ray ray = new Ray(node.Value.transform.position + Vector3.up, (transform.position - (node.Value.transform.position + Vector3.up)).normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction * 100, out hit, 100f))
            {
                if (hit.collider.transform == transform)
                {
                    seeableNodes.Add(node);
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
                }               
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 1f);
            }
        }

        int smallestCost = int.MaxValue;
        Node<MonoNode> bestNode = seeableNodes[0];

        //Now we had a list of seeable Nodes -- time to find the best node out of all of them
        foreach (Node<MonoNode> seeableNode in seeableNodes)
        {
            var camefrom = GraphSearch<MonoNode>.BFS(graph, seeableNode, goal);
            Node<MonoNode> currentNode = goal;

            int totalCost = 0;

            while (currentNode != seeableNode)
            {
                totalCost += currentNode.Value.cost;

                camefrom.TryGetValue(currentNode, out currentNode);
            }

            if(totalCost < smallestCost)
            {
                bestNode = seeableNode;
                smallestCost = totalCost;
            }
        }

        return bestNode;
    }
}
