using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelectorNode : BTNodeWithChildren
{
    public BTSelectorNode()
    {
    }

    public BTSelectorNode(List<BTNode> childList)
    {
        childNodes = childList;
    }

    public override NodeStates Evaluate()
    {
        foreach (BTNode node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.Fail:
                    nodeState = NodeStates.Fail;
                    continue;
                case NodeStates.Success:
                    nodeState = NodeStates.Success;
                    return nodeState;
                case NodeStates.Running:
                    nodeState = NodeStates.Running;
                    return nodeState;
                default:
                    nodeState = NodeStates.Success;
                    return nodeState;
            }            
        }
        return nodeState;
    }



}
