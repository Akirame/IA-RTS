using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Node> nodes;
    public GameObject nodito;
    public GameObject noditoObs;
    public GameObject noditoPosDebug;
    public Transform startPos;
    public float heightMap;
    public float widthMap;
    public int granularity;
    public LayerMask rayMask;
    public LayerMask nodeMask;
    public bool instanceDebug = false;
    public Pathfinder pathfinder = new Pathfinder();
    public bool linkDiagonals = false;
    List<Node> path = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
        LinkNodes();
        SetNodesToPathfinder();
    }

    private void SetNodesToPathfinder()
    {
        pathfinder.SetNodes(nodes);
    }

    private void CreateNodes()
    {
        nodes = new List<Node>();
        int heightNodeCount = (int)heightMap * granularity;
        int widthNodeCount = (int)widthMap * granularity;
        float distWidth = widthMap / widthNodeCount;
        float distHeight = heightMap / heightNodeCount;
        for (int i = 0; i < heightNodeCount; i++)
        {
            for (int j = 0; j < widthNodeCount; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(startPos.position + new Vector3(j* distWidth, i* distHeight, -1), Vector3.forward,out hit, 3, rayMask))
                {
                    Node node;
                    int nodeCost = UnityEngine.Random.Range(1, 10);
                    if (hit.transform.tag == "Ground")
                        node = new Node(hit.point, false, nodeCost);
                    else
                        node = new Node(hit.point, true, 100);
                    if (instanceDebug)
                    {
                        if (!node.obstacle)
                        {
                            GameObject noDebug = Instantiate(nodito, hit.point, Quaternion.identity);
                            noDebug.GetComponent<NodoDebug>().SetCost(nodeCost);
                        }
                        else
                            Instantiate(noditoObs, hit.point, Quaternion.identity);
                    }
                    nodes.Add(node);
                }
            }
        }
    }

    private void LinkNodes()
    {
        int widthNodeCount = (int)widthMap * granularity;
        float distWidth = widthMap / widthNodeCount;
        if (linkDiagonals)
            distWidth = Mathf.Sqrt(2 * (distWidth * distWidth));
        foreach (var node in nodes)
        {
            foreach (var nodeAdj in nodes)
            {
                if (node != nodeAdj)
                {
                    float dist = Math.Abs(Vector2.Distance(node.pos, nodeAdj.pos));
                    if (dist <= distWidth)
                    {
                        if (!Physics.Raycast(node.pos, (nodeAdj.pos - node.pos).normalized, dist, nodeMask))
                            node.adjacents.Add(nodeAdj);
                    }
                }
            }
        }
    }

    public List<Vector2> GetPathBetweenPos(Vector2 pos1, Vector2 pos2)
    {
        Node node1 = pathfinder.GetClosestTo(pos1);
        Node node2 = pathfinder.GetClosestTo(pos2);
        if (instanceDebug)
        {
            if (node1 != null)
                Instantiate(noditoPosDebug, node1.pos, Quaternion.identity);
            if (node2 != null)
                Instantiate(noditoPosDebug, node2.pos, Quaternion.identity);
        }
        return pathfinder.GetPathBetweenPos(pos1, pos2);
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPos.position, startPos.position + Vector3.right * widthMap);
        Gizmos.DrawLine(startPos.position, startPos.position + Vector3.up * heightMap);
        Gizmos.DrawLine(startPos.position + new Vector3(0, 0, -1), startPos.position + new Vector3(0, 0, 1) + Vector3.forward * heightMap);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos.position, (startPos.position + Vector3.up * heightMap / heightMap * granularity));
        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (i + 1 < path.Count)
                {
                    Gizmos.DrawLine((Vector3)path[i].pos + new Vector3(0,0,-3), (Vector3)path[i + 1].pos + new Vector3(0, 0, -3));
                }
            }
        }
    }

}
