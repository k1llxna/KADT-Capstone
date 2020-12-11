using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoNode : MonoBehaviour
{
    public List<MonoNode> Neighbours;

    public Node<Transform> node;

    public int cost;

}
