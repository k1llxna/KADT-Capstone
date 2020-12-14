using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class blah : MonoBehaviour
{

    public MonoNode[] nodes;
    public GameObject[] objects;

    public MonoNode start;
    public MonoNode finish;

    Node<MonoNode> startNode;
    Node<MonoNode> finishNode;

    public Graph<MonoNode> graph = new Graph<MonoNode>();

    public NodeList<MonoNode> nodeSet = new NodeList<MonoNode>();

    private void Start()
    {
        nodes = FindObjectsOfType<MonoNode>();

        foreach (MonoNode o in nodes)
        {
            GraphNode<MonoNode> newNode = new GraphNode<MonoNode>(o);
            newNode.Value = o;
            nodeSet.Add(newNode);
        }

        setStart(start);
        setFinish(finish);

        
        objects = GameObject.FindGameObjectsWithTag("Player");

        UpdateGraph();
    }

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine("Pathfind");
        }
        else if (Input.GetKeyDown("r"))
        {
            ResetGraph();
        }
        if (Input.GetKeyDown("e"))
        {
            StopAllCoroutines();
            ResetGraph();
        }

        foreach (Node<MonoNode> node in nodeSet)
        {
            if (node.Value.isEnabled)
            {
                foreach (Node<MonoNode> neighbourNode in graph.Neighbours(node))
                {
                    Debug.DrawLine(node.Value.transform.position, neighbourNode.Value.transform.position, Color.blue);
                }
            }
        }
    }


    public void setStart(MonoNode start)
    {
        this.start.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        this.start = start;
        this.start.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        startNode = nodeSet.FindByValue(start);
    }

    public void setFinish(MonoNode finish)
    {
        this.finish.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        this.finish = finish;
        this.finish.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        finishNode = nodeSet.FindByValue(finish);
    }
    public void setWall(MonoNode node)
    {
        if(node.tag.Equals("Player"))
        {
            node.tag = "Enemy";
            node.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            node.Disable();
        }
        else
        {
            node.tag = "Player";
            node.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            node.Enable();
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
        graph.Reset();

        graph = new Graph<MonoNode>();

        graph = new Graph<MonoNode>(nodeSet);

        foreach(GraphNode < MonoNode > gNode in nodeSet)
        {
            if (gNode.Value.isEnabled)
            {
                foreach (MonoNode mNode in gNode.Value.TrueNeighbours)
                {

                    GraphNode<MonoNode> neghbouringNode = (GraphNode<MonoNode>)nodeSet.FindByValue(mNode);

                    float cost = (mNode.cost + gNode.Value.cost);

                    graph.AddDirectedEdge(gNode, neghbouringNode, Mathf.RoundToInt(cost));

                }
            }
        }
    }

    IEnumerator Pathfind()
    {
        Debug.Log("Starting the search");

        var cameFrom = GraphSearch.ASearch(graph, startNode, finishNode);

        Debug.Log("Searched");

        Node<MonoNode> currentNode = finishNode;
        cameFrom.TryGetValue(currentNode, out currentNode);

        do
        {
            //Break if at the starting point -- Pathfound from destination to starting location
            if (currentNode == startNode)
                break;

            Debug.Log("MadeIT");

            print(currentNode);

            //Show Path
            currentNode.Value.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

            yield return new WaitForSeconds(0.2f);


            cameFrom.TryGetValue(currentNode, out currentNode);

        } while (currentNode != startNode);
        

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
