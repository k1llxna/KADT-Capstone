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

    public void Reset()
    {
        nodeSet.Clear();        
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
        from.Neighbours.Add(to);
        from.Costs.Add(cost);
    }

    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        AddDirectedEdge(from, to, cost); //This was duplicated so just call the existing value

        to.Neighbours.Add(to);
        to.Costs.Add(cost);
    }

    public int Cost(GraphNode<T> from, GraphNode<T> to)
    {
        int neighbourCounter = 0;

        foreach(GraphNode<T> neighbour in from.Neighbours)
        {
            if(neighbour == to)
            {
                return to.Costs[neighbourCounter];
            }
        }

        return 0;
    }

    public bool contains(T value)
    {
        return nodeSet.FindByValue(value) != null;
    }

    public NodeList<T> Neighbours(Node<T> node)
    {
        return nodeSet[nodeSet.IndexOf(node)].Neighbours;
    }

    public NodeList<T> Neighbours(T value)
    {
        return nodeSet.FindByValue(value).Neighbours;
    }

    public GraphNode<T> GetGraphNode(Node<T> node)
    {
        foreach(Node<T> n in  nodeSet)
        {
            if(n == node)
            {
                return (GraphNode<T>)n;
            }
        }

        return null;
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
            int index = gnode.Neighbours.IndexOf(nodeToRemove);
            if (index != -1)
            {
                gnode.Neighbours.RemoveAt(index);
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
