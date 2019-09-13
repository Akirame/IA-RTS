using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConditionNode : BTNode
{

    public bool condition;

    public override NodeStates Evaluate()
    {
        if (condition)
        {
            nodeState = NodeStates.Success;
        }
        else
        {
            nodeState = NodeStates.Fail;
        }
        return nodeState;
    }
}
