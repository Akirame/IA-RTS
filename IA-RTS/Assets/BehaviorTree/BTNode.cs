using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public enum NodeStates {None, Running, Success, Fail}
    protected string nodeName;
    protected NodeStates nodeState;
    protected bool canHaveChildren = false;

    public BTNode() { }

    public NodeStates GetNodeState()
    {
        return nodeState;
    }

    public delegate NodeStates NodeStateReturn();

    public abstract NodeStates Evaluate();

}
