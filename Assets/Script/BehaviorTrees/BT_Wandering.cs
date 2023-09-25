using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Wandering : BT
{
    public BT_Call WanderingAction;

    public BT_Wandering() : base(null) { }

    public void BuildBT()
    {
        // creation of the BT using data
        Root = new BT_Action(WanderingAction);
    }
}
