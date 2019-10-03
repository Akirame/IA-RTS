using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConditionNode : BTNode
{
    public delegate bool BTConditionDelegate();
    public BTConditionDelegate Condition;

    public BTConditionNode(BTConditionDelegate _condition)
    {
        Condition += _condition;
    }
    
    public override NodeStates Evaluate()
    {
        if (Condition())
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
