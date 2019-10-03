using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeWithChildren : BTNode
{
    protected List<BTNode> childNodes = new List<BTNode>();
    public void AddChild(BTNode node) { childNodes.Add(node); }
    public override NodeStates Evaluate() { return NodeStates.None; }
}
