using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGraph : MonoBehaviour
{
    public MonoNode[] nodes;

    public Graph<MonoNode> graph;
    public NodeList<MonoNode> nodeSet = new NodeList<MonoNode>();

    public Color neighbourColor;

    public GameObject blocker;

    public MonoNode p;

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
            if (node.Value.isEnabled)
            {
                foreach (Node<MonoNode> neighbourNode in graph.Neighbours(node))
                {
                    Debug.DrawLine(node.Value.transform.position, neighbourNode.Value.transform.position, neighbourColor);
                }
            }
        }


        if (Input.GetKeyDown("f"))
        {
            p.Disable();
            blocker.SetActive(true);
            UpdateGraph();
        }

        if (Input.GetKeyDown("g"))
        {
            p.Enable();
            blocker.SetActive(false);
            UpdateGraph();
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

        foreach(Node<MonoNode> node in nodeSet)
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
        
        foreach(Node<MonoNode> node in closestNode.Neighbors)
        {
            seeableNodes.Add(node);
        }

        foreach (Node<MonoNode> node in seeableNodes)
        {
            print(node.Value);
        }

        int smallestCost = int.MaxValue;
        Node<MonoNode> bestNode = seeableNodes[0];

        //Now we had a list of seeable Nodes -- time to find the best node out of all of them
        foreach (Node<MonoNode> seeableNode in seeableNodes)
        {
            if (seeableNode.Value.isEnabled)
            {
                var camefrom = GraphSearch<MonoNode>.Dijkstra(graph, seeableNode, goal);
                Node<MonoNode> currentNode = goal;

                int totalCost = 0;

                while (currentNode != seeableNode)
                {
                    totalCost += currentNode.Value.cost;

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
}
