using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequenceNode : BTNodeWithChildren
{

    public BTSequenceNode(List<BTNode> nodeList)
    {
        childNodes = nodeList;
    }

    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (BTNode node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.Fail:
                    nodeState = NodeStates.Fail;
                    return nodeState;
                case NodeStates.Success:
                    continue;
                case NodeStates.Running:
                    anyChildRunning = true;
                    continue;
                default:
                    nodeState = NodeStates.Success;
                    return nodeState;
            }
        }
        nodeState = anyChildRunning ? NodeStates.Running : NodeStates.Success;
        return nodeState;
    }

}
