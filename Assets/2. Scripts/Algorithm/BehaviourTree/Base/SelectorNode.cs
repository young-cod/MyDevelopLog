using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    public SelectorNode() : base() { }
    public SelectorNode(List<Node> children) : base(children) { }

    public override ENODESTATE Evaluate()
    {
        foreach (var child in childrenNode)
        {
            switch (child.Evaluate())
            {
                case ENODESTATE.Failure:
                    continue;
                case ENODESTATE.Success:
                    return state = ENODESTATE.Success;
                case ENODESTATE.Running:
                    return state = ENODESTATE.Running;
                default:
                    continue;
            }
        }
        return state = ENODESTATE.Failure;
    }
}
