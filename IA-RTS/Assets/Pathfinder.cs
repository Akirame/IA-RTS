using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinder
{
    public enum PathFindingType { Breath = 0, Depth };
    public PathFindingType findingPath = PathFindingType.Breath;
    public List<Node> openNodes = new List<Node>();
    public List<Node> closedNodes = new List<Node>();

    public List<Node> GetPath(Node source, Node destination)
    {
        List<Node> path = new List<Node>();
        OpenNode(source, null);
        while (openNodes.Count > 0)
        {
            Node node = VisitNode();
            if (node == destination)
            {
                path = ReturnPath(node);
                return path;
            }
            else
            {
                CloseNode(node);
                OpenAdjacents(node);
            }
        }
        openNodes.Clear();
        closedNodes.Clear();
        return path;
    }

    public void GetPathBetween(Vector3 pos1, Vector3 pos2)
    {
        
    }

    private List<Node> ReturnPath(Node node)
    {
        List<Node> path = new List<Node>();
        path.Add(node);
        Node parent = node.parent;
        do
        {
            path.Add(parent);
            parent = parent.parent;
        } while (parent != null);
        return path;
    }

    private void OpenAdjacents(Node node)
    {
        foreach (Node childNode in node.adjacents)
        {
            OpenNode(childNode, node);
        }
    }

    private void CloseNode(Node node)
    {
        if (node.state == Node.NodeState.Open)
        {
            openNodes.Remove(node);
            closedNodes.Add(node);
            node.state = Node.NodeState.Close;
        }
    }

    private Node VisitNode()
    {
        switch (findingPath)
        {
            case PathFindingType.Breath:
                return openNodes[0];
            case PathFindingType.Depth:
                return openNodes[openNodes.Count - 1];
            default:
                return null;
        }
    }

    private void OpenNode(Node source, Node parent)
    {
        if (source.state == Node.NodeState.None)
        {
            openNodes.Add(source);
            source.parent = parent;
            source.state = Node.NodeState.Open;
        }
    }
}
