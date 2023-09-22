using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM : HSM_State
{
    // standard initial state
    public HSM_State InitialState;

    // current state
    public HSM_State CurrentState;

    public HSM(Enum name, HSM_State initialState, int hierarchyLevel) : base(name, hierarchyLevel)
    {
        InitialState = initialState;
    }

    public HSM(Enum name, HSM initialState, int hierarchyLevel) : base(name, hierarchyLevel)
    {
        InitialState = initialState;
    }

    // recursively update of the machine
    public override HSM_Result Update()
    {
        // entering the state without a current state -> standard initial state
        if (CurrentState == null)
        {
            CurrentState = InitialState;

            // Execute actions associated to enter from the new current state
            CurrentState.Enter();

            // if current state is null -> it is a new entry in the current state -> init all children states
            CurrentState.Update();

            return null;
        }

        // searching a transition in current state
        HSM_Transition nextTran = CurrentState.VerifyTransitions();

        HSM_Result result;

        // if transaction founded in current state -> make result
        if (nextTran != null)
        {
            HSM_State nextState = CurrentState.NextState(nextTran);
            int level = CurrentState.GetLevel(nextTran);

            result = new HSM_Result(nextState, nextTran, level);
        }
        // else search in lower levels
        else
        {
            result = CurrentState.Update();
        }

        // need to to something
        if (result != null)
        {
            if (result.Level == 0)
            {
                // next state is in current level

                /*
                // first exit and delete lower states
                if (CurrentState as HSM != null)
                    CurrentState.UpdateUp((CurrentState as HSM).CurrentState);
                */

                // exit from current state
                CurrentState.Exit();
                // fire transition from result (it can be either from current state or from lower ones
                result.NextTransition.Fire();
                // set target state (which is at this level) as current because level = 0
                CurrentState = result.NextState;
                // enter in new state
                CurrentState.Enter();

                // clean result to stop the search in next step
                result = null;
            }
            else if (result.Level > 0)
            {
                // next state is at higher level

                // exit from current state
                CurrentState.Exit();

                // the destination is higher level, so current state is lost
                CurrentState = null;

                // next step is one step closer to target level
                result.Level--;
            }
            else // result.Level < 0
            {
                // next state is at lower level

                // not recursive step of Update()

                // fire transition from result (it can be either from current state or from lower ones
                result.NextTransition.Fire();

                // get the parent state for update up to current hierarchy level
                HSM_State parentState = result.NextState.ParentState;
                parentState.UpdateDown(result.NextState, -result.Level);

                // clean result to stop the search in next step
                result = null;
            }
        }
        else
        {
            // basic current state actions
            CurrentState.Stay();
        }

        return result;
    }

    public override void UpdateDown(HSM_State nextState, int level)
    {
        // if not in beginning level -> continue recursing
        if (level > 0)
            this.ParentState.UpdateDown(this, level - 1);

        // exit from current state if exists
        if (CurrentState != null)
            CurrentState.Exit();

        // new state is the parent of next state
        CurrentState = nextState;
        // entering each parent state
        CurrentState.Enter();
    }

    public override void UpdateUp(HSM_State lowerState)
    {
        // if not last level -> continue recursing
        if (lowerState as HSM != null)
            this.CurrentState.UpdateUp(this.CurrentState);

        // exit from current state if exists
        if (CurrentState != null)
            CurrentState.Exit();

        // delete current state
        CurrentState = null;
    }
}
