using System.Collections.Generic;
using Priority_Queue;


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

    public static Dictionary<Node<T>, Node<T>> Dijkstra(Graph<T> graph, Node<T> start, Node<T> goal)
    {
        var frontier = new SimplePriorityQueue<Node<T>, int>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        var costSoFar = new Dictionary<Node<T>, int>();

        cameFrom[start] = default(Node<T>);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Node<T> current = frontier.Dequeue();

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

    public static Dictionary<Node<T>, Node<T>> ASearch(Graph<T> graph, Node<T> start, Node<T> goal)
    {
        var frontier = new SimplePriorityQueue<Node<T>, int>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        var costSoFar = new Dictionary<Node<T>, int>();

        cameFrom[start] = default(Node<T>);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Node<T> current = frontier.Dequeue();

            foreach (var next in graph.Neighbours(current))
            {
                int newCost = costSoFar[current] + graph.Cost(graph.GetGraphNode(current), graph.GetGraphNode(next));

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost;// + Heuristic(next.Value, goal.Value);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return cameFrom;
    }

    int Heuristic(Graph<T> graph, Node<T> start, Node<T> finish)
    {
        return 0;
    }
}
