using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch<T>
{
    public static Dictionary<Node<T>, Node<T>> BFS(Graph<T> graph, Node<T> start, Node<T> goal)
    {
        var frontier = new Queue<Node<T>>();
        frontier.Enqueue(start);

        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        
        cameFrom[start] = default(Node<T>);

        while (frontier.Count > 0)
        {
            Node<T> current = frontier.Dequeue();

            if(current == goal)
                break;

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

    public static Dictionary<Node<T>, Node<T>> GreedyBFS(Graph<T> graph, Node<T> start, Node<T> goal)
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

    public static Dictionary<Node<T>, Node<T>> Dijkstra(Graph<T> graph, Node<T> start, Node<T> goal)
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

    public static Dictionary<Node<T>, Node<T>> ASearch(Graph<T> graph, Node<T> start, Node<T> goal)
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

    float heuristic(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    float heuristic(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }
}
