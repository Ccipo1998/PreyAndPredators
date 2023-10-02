using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_DecoratorUntilFail : BT_Decorator
{
    public BT_DecoratorUntilFail(BT_ITask child) : base(child) { }

    public override int Run()
    {
        if (_child.Run() != 0)
            return -1;

        return 1;
    }
}
