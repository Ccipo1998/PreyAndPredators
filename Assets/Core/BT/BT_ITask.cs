using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate int BT_Call();

public abstract class BT_ITask
{
    // return values meaning:
    // 0: fail
    // 1: success
    // -1: call me again
    public abstract int Run();
}
