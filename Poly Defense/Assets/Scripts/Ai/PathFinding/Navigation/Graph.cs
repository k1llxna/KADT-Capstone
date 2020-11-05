using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    NodeList<T> nodeSet;
    public NodeList<T> Nodes { get; }

    public Graph()
    {
        nodeSet = new NodeList<T>();
    }
    public Graph(NodeList<T> nodeSet)
    {
        if(nodeSet == null)
        {
            this.nodeSet = new NodeList<T>();
        }
        else
        {
            this.nodeSet = nodeSet;
        }
    }

    public void AddNode(GraphNode<T> node)
    {
        nodeSet.Add(node);
    }
    public void AddNode(T value)
    {
        nodeSet.Add(new GraphNode<T>(value));
    }


    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        from.Neighbors.Add(to);
        from.Costs.Add(cost);
    }

    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        AddDirectedEdge(from, to, cost); //This was duplicated so just call the existing value

        to.Neighbors.Add(to);
        to.Costs.Add(cost);
    }

    public bool contains(T value)
    {
        return nodeSet.FindByValue(value) != null;
    }

    public NodeList<T> Neighbours(Node<T> node)
    {
        return nodeSet[nodeSet.IndexOf(node)].Neighbors;
    }

    public NodeList<T> Neighbours(T value)
    {
        return nodeSet.FindByValue(value).Neighbors;
    }

    public bool Remove(T value)
    {
        // Remove node from nodeset
        GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
        if (nodeToRemove == null)
        {
            // wasnt found
            return false;
        }

        // was found
        nodeSet.Remove(nodeToRemove);

        // enumerate through each node in nodeSet, removing edges to this node
        foreach (GraphNode<T> gnode in nodeSet)
        {
            int index = gnode.Neighbors.IndexOf(nodeToRemove);
            if (index != -1)
            {
                gnode.Neighbors.RemoveAt(index);
                gnode.Costs.RemoveAt(index);
            }
        }

        return true;
    }

    public int Count
    {
        get { return nodeSet.Count; }
    }
}
