using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Condition : BT_ITask
{
    public BT_Call Condition;

    public BT_Condition(BT_Call condition)
    {
        Condition = condition;
    }

    public override int Run()
    {
        return Condition() ? 1 : 0;
    }
}
