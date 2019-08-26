using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Node> nodes;
    public List<Node> openNodes;
    public GameObject nodito;
    public GameObject noditoObs;
    public Transform startPos;
    public int heightMap;
    public int widthMap;
    public int granularity;
    public LayerMask rayMask;
    public LayerMask nodeMask;
    public bool instanceDebug = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateNodes();
        LinkNodes();
    }

    private void CreateNodes()
    {
        nodes = new List<Node>();
        int heightNodeCount = heightMap * granularity;
        int widthNodeCount = widthMap * granularity;
        for (int i = 0; i < widthNodeCount; i++)
        {
            for (int j = 0; j < heightNodeCount; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(startPos.position + new Vector3((float)i/granularity, (float)j /granularity, -1), Vector3.forward,out hit, 3, rayMask))
                {
                    Node node;
                    if (hit.transform.tag == "Ground")
                        node = new Node(hit.point, false);
                    else
                        node = new Node(hit.point, true);
                    if (instanceDebug)
                    {
                        if (!node.obstacle)
                            Instantiate(nodito, hit.point, Quaternion.identity);
                        else
                            Instantiate(noditoObs, hit.point, Quaternion.identity);
                    }
                    nodes.Add(node);
                    print(hit);
                }
            }
        }
    }
    private void LinkNodes()
    {
        foreach (var node in nodes)
        {
            foreach (var nodeAdj in nodes)
            {
                if (node != nodeAdj)
                {
                    float dist = Math.Abs(Vector2.Distance(node.pos, nodeAdj.pos));
                    if (dist <= widthMap / granularity)
                    {
                        if (!Physics.Raycast(node.pos, (nodeAdj.pos - node.pos).normalized, dist, nodeMask))
                            node.adjacents.Add(nodeAdj);                        
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            print(nodes[0].adjacents.Count);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPos.position, startPos.position + Vector3.right * widthMap);
        Gizmos.DrawLine(startPos.position, startPos.position + Vector3.up * heightMap);
        Gizmos.DrawLine(startPos.position + new Vector3(0, 0, -1), startPos.position + new Vector3(0, 0, 1) + Vector3.forward * heightMap);

    }

}
