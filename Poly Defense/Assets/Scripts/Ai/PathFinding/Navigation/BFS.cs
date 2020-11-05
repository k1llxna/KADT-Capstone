using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch<T>
{
    public static Dictionary<Node<T>, Node<T>> Search(Graph<T> graph, Node<T> start)
    {
        var frontier = new Queue<Node<T>>();
        frontier.Enqueue(start);

        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        
        cameFrom[start] = default(Node<T>);

        while (frontier.Count > 0)
        {
            Node<T> current = frontier.Dequeue();

            foreach (var next in graph.Neighbours(current))
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        return cameFrom;
    }
}
