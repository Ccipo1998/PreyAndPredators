using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM_Zebra : HSM
{
    public HSM_Zebra(BT_Search BTSearch) : base(string.Empty, null, -1)
    {
        // hard-coded zebra HSM
        HSM_State search = new HSM_State("Search", 0);
        search.EnterActions.Add(BTSearch.StartBT);
        InitialState = new HSM("Zebra HSM", search, 0);
    }
}
