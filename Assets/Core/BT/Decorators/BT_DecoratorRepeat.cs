using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_DecoratorRepeat : BT_Decorator
{
    public BT_DecoratorRepeat(BT_ITask child) : base(child) { }

    public override int Run()
    {
        _child.Run();

        return -1;
    }
}
