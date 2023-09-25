using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Search : BT
{
    public BT_Call WanderingAction;

    // wait data
    private bool wait = false;
    private float time = .0f;
    private float waited = .0f;
    private float waitFor;

    public BT_Search(float waitFor = 5.0f) : base(null)
    {
        this.waitFor = waitFor;
    }

    public override void BuildBT()
    {
        List<BT_ITask> children = new List<BT_ITask>
        {
            new BT_Action(WanderingAction),
            new BT_Action(Wait)
        };
        BT_Sequence sequence = new BT_Sequence(children);
        BT_DecoratorUntilFail until = new BT_DecoratorUntilFail(sequence);

        Root = sequence;
    }

    private int Wait()
    {
        // first call
        if (!wait)
        {
            wait = true;
            time = Time.time;
            waited = .0f;

            return -1;
        }
        
        // wait done
        if (waited >= waitFor)
        {
            wait = false;

            return 1;
        }

        // still waiting
        waited += Time.time - time;
        time = Time.time;

        return -1;
    }
}
