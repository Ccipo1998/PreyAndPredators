using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Search : BT_MonoBehavior
{
    [SerializeField]
    private AI_Animal _AIAnimal;

    [SerializeField]
    private MovementMechanic _MovementMechanic;

    private void OnEnable()
    {
        // create BT

        List<BT_ITask> children = new List<BT_ITask>
        {
            new BT_Action(_MovementMechanic.RandomWalk),
            new BT_Action(Wait)
        };
        BT_Sequence sequence = new BT_Sequence(children);
        BT_DecoratorUntilFail until = new BT_DecoratorUntilFail(sequence);

        Root = sequence;

        // at end of initialization, always notify that
        _AIAnimal.BT_Initialized();
    }

    // stop movement just after stopping the BT
    protected override void OnStopBT()
    {
        _MovementMechanic.Stop();
    }

    #region ACTIONS

    private bool wait = false;
    private float time = .0f;
    private float waited = .0f;
    private float waitFor = 5.0f;

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

    #endregion
}
