using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBTAI : TreeBase
{
    protected override Node InitializeSetting()
    {
        Node root = new SelectorNode(new List<Node>{
            new SequenceNode(new List<Node>{
                 new SequenceNode(new List<Node>
                {
                    //new CheckPlayerIsNear(pet),
                    //new StayNearPlayerNode(pet)
                }),
                //new GoToPlayerNode(player, pet)
            })
        });

        return root;
    }
}
