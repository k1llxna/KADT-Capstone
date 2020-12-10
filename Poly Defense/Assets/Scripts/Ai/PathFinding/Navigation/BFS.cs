using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class GraphSearch
{
    public static Dictionary<Node<MonoNode>, Node<MonoNode>> BFS(Graph<MonoNode> graph, Node<MonoNode> start, Node<MonoNode> goal)
    {
        var frontier = new Queue<Node<MonoNode>>();
        frontier.Enqueue(start);

        var cameFrom = new Dictionary<Node<MonoNode>, Node<MonoNode>>();
        
        cameFrom[start] = default(Node<MonoNode>);

        while (frontier.Count > 0)
        {
            Node<MonoNode> current = frontier.Dequeue();

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

    public static Dictionary<Node<MonoNode>, Node<MonoNode>> GreedyBFS(Graph<MonoNode> graph, Node<MonoNode> start, Node<MonoNode> goal)
    {
        var frontier = new Queue<Node<MonoNode>>();
        frontier.Enqueue(start);

        var cameFrom = new Dictionary<Node<MonoNode>, Node<MonoNode>>();

        cameFrom[start] = default(Node<MonoNode>);

        while (frontier.Count > 0)
        {
            Node<MonoNode> current = frontier.Dequeue();

            if (current == goal)
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

    public static Dictionary<Node<MonoNode>, Node<MonoNode>> Dijkstra(Graph<MonoNode> graph, Node<MonoNode> start, Node<MonoNode> goal)
    {
        var frontier = new SimplePriorityQueue<Node<MonoNode>, int>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node<MonoNode>, Node<MonoNode>>();
        var costSoFar = new Dictionary<Node<MonoNode>, int>();

        cameFrom[start] = default(Node<MonoNode>);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Node<MonoNode> current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var next in graph.Neighbours(current))
            {
                int newCost = costSoFar[current] + graph.Cost(graph.GetGraphNode(current), graph.GetGraphNode(next));

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    frontier.Enqueue(next, newCost);
                    
                }
            }
        }

        return cameFrom;
    }

    public static Dictionary<Node<MonoNode>, Node<MonoNode>> ASearch(Graph<MonoNode> graph, Node<MonoNode> start, Node<MonoNode> goal)
    {
        var frontier = new SimplePriorityQueue<Node<MonoNode>, float>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node<MonoNode>, Node<MonoNode>>();
        var costSoFar = new Dictionary<Node<MonoNode>, float>();

        cameFrom[start] = default(Node<MonoNode>);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Node<MonoNode> current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var next in graph.Neighbours(current))
            {
                float newCost = costSoFar[current] + graph.Cost(graph.GetGraphNode(current), graph.GetGraphNode(next));

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next.Value.transform, goal.Value.transform);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return cameFrom;
    }

    static float Heuristic(Transform start, Transform finish)
    {
        return (start.position - finish.position).magnitude;
    }
}
