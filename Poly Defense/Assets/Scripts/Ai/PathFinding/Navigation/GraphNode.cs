using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode<T> : Node<T>
{
    private List<int> costs;

    public GraphNode() : base() { }
    public GraphNode(T value) : base(value) { }
    public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

    new public NodeList<T> Neighbours
    {
        get
        {
            if (base.Neighbours == null)
                base.Neighbours = new NodeList<T>();

            return base.Neighbours;
        }
    }

    public List<int> Costs
    {
        get
        {
            if (costs == null)
                costs = new List<int>();

            return costs;
        }
    }
}
