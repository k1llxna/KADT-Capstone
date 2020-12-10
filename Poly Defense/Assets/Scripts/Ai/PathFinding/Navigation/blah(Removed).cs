/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class blah : MonoBehaviour
{

    public GameObject[] objects;

    public Transform start;
    public Transform finish;

    Node<Transform> startNode;
    Node<Transform> finishNode;

    public Graph<Transform> graph;

    public NodeList<Transform> nodeSet = new NodeList<Transform>();

    private void Start()
    {
        setStart(start);
        setFinish(finish);
        UpdateGraph();
    }

    private void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            StartCoroutine("Pathfind");
        }
        else if(Input.GetKeyDown("r"))
        {
            ResetGraph();
        }
    }

    public void setStart(Transform start)
    {
        this.start.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        this.start = start;
        this.start.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void setFinish(Transform finish)
    {
        this.finish.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        this.finish = finish;
        this.finish.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void setWall(Transform node)
    {
        if(node.tag.Equals("Player"))
        {
            node.tag = "Enemy";
            node.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else
        {
            node.tag = "Player";
            node.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        UpdateGraph();
    }

    private void ResetGraph()
    {
        foreach(GameObject o in objects)
        {
            if(o.tag.Equals("Player") && o.transform != start && o.transform != finish)
            {
                o.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void UpdateGraph()
    {

        nodeSet.Clear();

        objects = GameObject.FindGameObjectsWithTag("Player");



        foreach (GameObject o in objects)
        {
            GraphNode<Transform> newNode = new GraphNode<Transform>(o.transform);
            newNode.Value = o.transform;
            nodeSet.Add(newNode);
        }


        graph = new Graph<Transform>(nodeSet);

        foreach (GraphNode<Transform> node in nodeSet)
        {
            foreach (GraphNode<Transform> node2 in nodeSet)
            {
                if (InBounds(node2.Value, node.Value))
                {
                    graph.AddDirectedEdge(node2, node, 1);
                }
            }


            if(node.Value == start)
            {
                startNode = node;
            }

            if(node.Value == finish)
            {
                finishNode = node;
            }
        }
    }

    IEnumerator Pathfind()
    {

        foreach(Node<Transform> t in nodeSet[105].Neighbours)
        {
            print(t.Value);
        }

        //Dictionary<Node<Transform>, Node<Transform>> cameFrom = GraphSearch.BFS(graph, startNode, finishNode);


        Node<Transform> nextNode;
        Node<Transform> currentNode = finishNode;
        cameFrom.TryGetValue(currentNode, out nextNode);

        do
        {
            //Break if at the starting point -- Pathfound from destination to starting location
            if (nextNode == startNode)
                break;

            //Show Path
            nextNode.Value.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            yield return new WaitForSeconds(0.2f);


            currentNode = nextNode;
            cameFrom.TryGetValue(currentNode, out nextNode);

        } while (nextNode != startNode);
        

    }

    bool InBounds(Transform a, Transform b)
    {
        if(Mathf.Abs(a.position.x - b.position.x) == 1 && a.position.y == b.position.y)
        {
            return true;
        }
        else if (Mathf.Abs(a.position.y - b.position.y) == 1 && a.position.x == b.position.x)
        {
            return true;
        }

        return false;
    }

}
*/