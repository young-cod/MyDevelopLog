using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENODESTATE
{
    Failure,
    Success,
    Running,
}

public abstract class Node
{
    protected ENODESTATE state;
    public Node parentNode;
    protected List<Node> childrenNode = new List<Node>();

    public Node() => parentNode = null;

    public Node(List<Node> children)
    {
        foreach (var child in children)
        {
            AttachChild(child);
        }
    }

    public void AttachChild(Node child)
    {
        childrenNode.Add(child);
        child.parentNode = this;
    }

    public abstract ENODESTATE Evaluate();
}
