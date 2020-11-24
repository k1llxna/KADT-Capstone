using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    private T data;
    private NodeList<T> neighbors = null;

    public Node() { }
    public Node(T data) : this(data, null) { }
    public Node(T data, NodeList<T> neighbors)
    {
        this.data = data;
        this.neighbors = neighbors;
    }

    public T Value { get; set; }

    public NodeList<T> Neighbors { get; set; }
}
