using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    private T data;
    private NodeList<T> neighbours = null;

    public Node() { }
    public Node(T data) : this(data, null) { }
    public Node(T data, NodeList<T> neighbours)
    {
        this.data = data;
        this.neighbours = neighbours;
    }

    public T Value { get; set; }

    public NodeList<T> Neighbours { get; set; } //Actual Neighbours
}
