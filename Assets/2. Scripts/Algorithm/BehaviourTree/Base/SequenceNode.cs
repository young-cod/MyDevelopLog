using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    SequenceNode() : base() { }
    SequenceNode(List<Node> children) : base(children) { }

    public override ENODESTATE Evaluate()
    {
        bool isRunning = false;

        foreach (var child in childrenNode)
        {
            switch (child.Evaluate())
            {
                case ENODESTATE.Failure:
                    return ENODESTATE.Failure;
                case ENODESTATE.Success:
                    continue;
                case ENODESTATE.Running:
                    isRunning = true;
                    continue;
                default:
                    continue;
            }
        }

        return state = isRunning ? ENODESTATE.Running : ENODESTATE.Success;
    }
}
