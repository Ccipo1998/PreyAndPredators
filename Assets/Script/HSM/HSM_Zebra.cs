using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM_Zebra : HSM
{
    // actions
    private MovementMechanic _movementMechanic;

    // BTs
    private BT_Wandering _BT_wandering;

    public HSM_Zebra(MovementMechanic movementMechanic) : base(string.Empty, null, -1)
    {
        _movementMechanic = movementMechanic;

        // hard-coded zebra HSM
        HSM_State wandering = new HSM_State("Wandering", 0);
        _BT_wandering = new BT_Wandering();
        _BT_wandering.WanderingAction = _movementMechanic.RandomWalk;
        _BT_wandering.BuildBT();
        wandering.StayActions.Add(_BT_wandering.Step);
        InitialState = new HSM("Zebra HSM", wandering, 0);
    }
}
