using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGraph : MonoBehaviour
{
    MonoNode[] nodes;

    public Graph<MonoNode> graph;
    public NodeList<MonoNode> nodeSet = new NodeList<MonoNode>();

    public Color neighbourColor;

    // Start is called before the first frame update
    void Start()
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

                float distance = (mNode.transform.position - gNode.Value.transform.position).magnitude;

                graph.AddDirectedEdge(gNode, neghbouringNode, Mathf.RoundToInt(distance));
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

    public Node<MonoNode> FindClosestNode(Transform transform)
    {
        Node<MonoNode> closestNode = nodeSet[0];

        foreach(Node<MonoNode> node in nodeSet)
        {
            if (Mathf.Abs((node.Value.transform.position - transform.position).magnitude) < Mathf.Abs((closestNode.Value.transform.position - transform.position).magnitude))
            {
                //If the node can see the transform it is a valid node -- Object cannot use a close node that is behind a wall
                Ray ray = new Ray(transform.position + Vector3.up, (node.Value.transform.position - (transform.position + Vector3.up)).normalized);
                RaycastHit hit;
                if(Physics.Raycast(ray.origin, ray.direction * 100, out hit, 100f))
                {
                    if(hit.point == node.Value.transform.position)
                    {
                        closestNode = node;
                        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
                    }                    
                }                
            }
        }
        return closestNode;
    }
}
