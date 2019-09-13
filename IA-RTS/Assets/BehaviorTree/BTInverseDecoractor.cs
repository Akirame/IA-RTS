using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInverseDecoractor : BTNode
{
    private BTNode childNode;

    public BTInverseDecoractor(BTNode node)
    {
        childNode = node;
    }

    public override NodeStates Evaluate()
    {
        switch (childNode.Evaluate())
        {
            case NodeStates.Fail:
                nodeState = NodeStates.Success;
                return nodeState;
            case NodeStates.Success:
                nodeState = NodeStates.Fail;
                return nodeState;
            case NodeStates.Running:
                nodeState = NodeStates.Running;
                return nodeState;
        }
        nodeState = NodeStates.Success;
        return nodeState;
    }
}
