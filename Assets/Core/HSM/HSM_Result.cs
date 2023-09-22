using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM_Result
{
    public HSM_Result(HSM_State nextState, HSM_Transition nextTransition, int level)
    {
        NextState = nextState;
        NextTransition = nextTransition;
        Level = level;
    }

    public HSM_State NextState;
    public HSM_Transition NextTransition;
    public int Level;
}
