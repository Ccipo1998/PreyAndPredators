using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_Composite
{
    private int index;

    public BT_Sequence(List<BT_ITask> children) : base(children)
    {
        index = 0;
    }

    public override int Run()
    {
        while (index < Children.Count)
        {
            int result = Children[index].Run();

            // child is running -> call again the sequence
            if (result == -1)
            {
                return -1;
            }

            // child has failed -> sequence fails
            if (result == 0)
            {
                index = 0;

                return 0;
            }

            // child had success -> call again sequence on next child
            if (result == 1)
            {
                ++index;

                return -1;
            }
        }

        // if sequence is called again and no child left -> success
        index = 0;

        return 1;
    }
}
