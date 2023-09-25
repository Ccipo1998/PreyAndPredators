using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Selector : BT_Composite
{
    private int index;

    public BT_Selector(List<BT_ITask> children) : base(children)
    {
        index = 0;
    }

    public override int Run()
    {
        while (index < Children.Count)
        {
            int result = Children[index].Run();

            // child is running -> need to call again the selector
            if (result == -1)
            {
                return -1;
            }

            // child failed -> update index to run next child and call again the selector
            if (result == 0)
            {
                ++index;

                return -1;
            }

            // child succeded -> reset index and return success
            if (result == 1)
            {
                index = 0;

                return 1;
            }
        }

        // if selector is called again and no child left -> success
        index = 0;

        return 1;
    }
}
