using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoNode : MonoBehaviour
{
    public List<MonoNode> Neighbours;
    public List<MonoNode> TrueNeighbours;

    public Node<Transform> node;

    public int cost;
    public bool isEnabled = true;

    private void Start()
    {
        foreach(MonoNode node in Neighbours)
        {
            TrueNeighbours.Add(node);
        }
    }

    public void Disable()
    {     
        foreach(MonoNode neigbour in Neighbours)
        {
            neigbour.TrueNeighbours.Remove(this);
        }

        TrueNeighbours.Clear();

        isEnabled = false;
        Debug.Log("I got disabled");
    }

    public void Enable()
    {
        foreach(MonoNode neighbour in Neighbours)
        {
            if(neighbour.isEnabled)
            {
                TrueNeighbours.Add(neighbour);
                neighbour.TrueNeighbours.Add(this);
            }
        }

        isEnabled = true;
        Debug.Log("I got enabled");
    }

}
