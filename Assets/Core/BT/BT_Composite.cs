using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_Composite : BT_ITask
{
    public List<BT_ITask> Children;

    public BT_Composite(List<BT_ITask> children)
    {
        Children = children;
    }
}
