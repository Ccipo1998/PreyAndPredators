using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all zebra's ai mechanics are handled here
public class AI_Zebra : AI_Animal
{
    [SerializeField]
    private MovementMechanic _MovementMechanic;
    [SerializeField]
    private BT_Search _BTSearch;

    // number of behavior trees associated to the AI -> for initialization
    private int _BT_number = 1;

    private void OnEnable()
    {
        // init zebra hsm
        _hsm = new HSM_Zebra(_BTSearch);
    }

    public override void BT_Initialized()
    {
        --_BT_number;

        if (_BT_number == 0)
            StartCoroutine(UpdateHSM());
    }
}
