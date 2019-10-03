using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequenceNode : BTNodeWithChildren
{
    public BTSequenceNode()
    {
    }

    public BTSequenceNode(List<BTNode> nodeList)
    {
        childNodes = nodeList;
    }
        

    public override NodeStates Evaluate()
    {        

        foreach (BTNode node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.Fail:
                    nodeState = NodeStates.Fail;
                    return nodeState;
                case NodeStates.Success:
                    nodeState = NodeStates.Success;
                    continue;
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
