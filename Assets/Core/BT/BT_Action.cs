using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Action : BT_ITask
{
    public BT_Call Action;

    public BT_Action(BT_Call action)
    {
        Action = action;
    }

    public override int Run()
    {
        return Action();
    }
}
