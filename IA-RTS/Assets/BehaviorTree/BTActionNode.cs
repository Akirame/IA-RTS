using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTActionNode : BTNode
{
    public delegate NodeStates ActionNodeDelegate();

    private ActionNodeDelegate action;

    public BTActionNode(ActionNodeDelegate _action)
    {
        action = _action;
    }

    public override NodeStates Evaluate()
    {
        switch (action())
        {
            case NodeStates.Success:
                nodeState = NodeStates.Success;
                return nodeState;
            case NodeStates.Fail:
                nodeState = NodeStates.Fail;
                return nodeState;
            case NodeStates.Running:
                nodeState = NodeStates.Running;
                return nodeState;
            default:
                nodeState = NodeStates.Fail;
                return nodeState;
        }
    }
}
