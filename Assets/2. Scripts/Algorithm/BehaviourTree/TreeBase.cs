using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreeBase : MonoBehaviour
{
    Node root;

    protected void Start()
    {
        root = InitializeSetting();
    }

    protected void Update()
    {
        if (root == null) return;
        root.Evaluate();
    }

    protected abstract Node InitializeSetting();

}
