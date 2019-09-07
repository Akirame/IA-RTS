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
    public List<Node> nodeList = new List<Node>();

    public List<Node> GetPath(Node source, Node destination)
    {
        List<Node> path = new List<Node>();
        OpenNode(source, null);
        while (openNodes.Count > 0 && path.Count <= 0)
        {
            Node node = VisitNode();
            if (node == destination)
            {
                path = ReturnPath(node);
                break;
            }
            else
            {
                CloseNode(node);
                OpenAdjacents(node);
            }
        }
        ClearLists();
        return path;
    }

    public void SetNodes(List<Node> nodes)
    {
        nodeList = nodes;
    }

    private void ClearLists()
    {
        foreach (Node node in openNodes)
        {
            node.state = Node.NodeState.None;
        }
        foreach (Node node in closedNodes)
        {
            node.state = Node.NodeState.None;
        }
        openNodes.Clear();
        closedNodes.Clear();
    }

    public List<Vector2> GetPathBetweenPos(Vector2 pos1, Vector2 pos2)
    {
        Node sourceNode = GetClosestTo(pos1);
        Node destNode = GetClosestTo(pos2);
        List<Vector2> posPath = new List<Vector2>();
        List<Node> path = new List<Node>();
        if (sourceNode != null && destNode != null)
        {
            path = GetPath(sourceNode, destNode);
            foreach (Node node in path)
            {
                posPath.Add(node.pos);
            }
        }
        return posPath;
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

    public Node GetClosestTo(Vector2 pos1)
    {
        Node nodeFound = null;
        float sizeFactor = 0.2f;
        float conta = 0;
        Rect rect = new Rect();
        while (nodeFound == null && conta < 5)
        {
            rect.xMin = pos1.x - sizeFactor;
            rect.xMax = pos1.x + sizeFactor;
            rect.yMin = pos1.y - sizeFactor;
            rect.yMax = pos1.y + sizeFactor;
            foreach (Node node in nodeList)
            {
                if (!node.obstacle && rect.Contains(node.pos) && nodeFound == null)
                {
                    nodeFound = node;
                    break;
                }
            }
            conta++;
            sizeFactor *= 2;
        }
        return nodeFound;
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
