using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT
{
    public BT_ITask Root;

    public BT(BT_ITask root)
    {
        Root = root;
    }

    public int StatusStep()
    {
        return Root.Run();
    }

    public void Step()
    {
        Root.Run();
    }

    // creation of specific BTs using data
    public abstract void BuildBT();
}
