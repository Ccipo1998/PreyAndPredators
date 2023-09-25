using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM_Zebra : HSM
{
    // actions
    private MovementMechanic _movementMechanic;

    // BTs
    //private BT_Wandering _BT_wandering;
    private BT_Search _BT_search;

    public HSM_Zebra(MovementMechanic movementMechanic) : base(string.Empty, null, -1)
    {
        _movementMechanic = movementMechanic;

        // hard-coded zebra HSM
        HSM_State search = new HSM_State("Search", 0);
        _BT_search = new BT_Search();
        _BT_search.WanderingAction = _movementMechanic.RandomWalk;
        _BT_search.BuildBT();
        search.StayActions.Add(_BT_search.Step);
        InitialState = new HSM("Zebra HSM", search, 0);
    }
}
