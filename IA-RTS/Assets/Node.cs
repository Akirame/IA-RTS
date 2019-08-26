using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum NodeState { Open,Close,None};
    public NodeState state = NodeState.None; 
    public Vector2 pos;
    public bool obstacle = false;
    public List<Node> adjacents;

    public Node(Vector2 _pos, bool _obst)
    {
        pos = _pos;
        obstacle = _obst;
        adjacents = new List<Node>();
    }

}
