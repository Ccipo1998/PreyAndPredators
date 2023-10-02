using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Satisfy : BT_MonoBehavior
{
    [Header("BT parameters")]
    private float _GoToResourceMaximumTime; // it is the maximum accepted time for the BT to arrive to the resource

    [SerializeField]
    private AI_Animal _AIAnimal;

    [SerializeField]
    private MovementMechanic _MovementMechanic;

    public Resource Resource;

    private float _timer = .0f;

    private void OnEnable()
    {
        // create BT

        // lower level
        List<BT_ITask> children2 = new List<BT_ITask>
        {
            new BT_Condition(IsResourceNotNull),
            new BT_Action(GoToResource),
            new BT_Action(StartTimer),
            new BT_Condition(IsArrivedToResource)
            //new BT_Action(Satisfy)
        };
        BT_Sequence sequence = new BT_Sequence(children2);

        // higher level
        List<BT_ITask> children1 = new List<BT_ITask>
        {
            sequence
            //new BT_Action(Satisfy)
        };
        BT_Selector selector = new BT_Selector(children1);

        Root = selector;

        // at end of initialization, always notify that
        _AIAnimal.BT_Initialized();
    }

    
    protected override void OnStopBT()
    {
        
    }

    private int IsResourceNotNull()
    {
        return Resource != null ? 1 : 0;
    }

    private int GoToResource()
    {
        _MovementMechanic.GoTo((Vector2)Resource.transform.position);

        return 0;
    }

    private int StartTimer()
    {
        _timer = Time.time;

        return 0;
    }

    private int IsArrivedToResource()
    {
        // success
        if ((Resource.transform.position - transform.position).magnitude <= _MovementMechanic.StopDistance)
        {
            // arrived
            return 1;
        }

        // fail
        if (Time.time - _timer >= _GoToResourceMaximumTime)
            return 0;

        // wait
        return -1;
    }
}