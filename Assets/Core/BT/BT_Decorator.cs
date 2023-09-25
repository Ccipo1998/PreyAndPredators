using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_Decorator : BT_ITask
{
    protected BT_ITask _child;

    public BT_Decorator(BT_ITask child)
    {
        _child = child;
    }
}
