using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSM_State
{
    // state name -> for sanity
    public Enum Name;

    // list of actions to perform based on transitions fire or not
    public List<HSM_Action> EnterActions = new List<HSM_Action>();
    public List<HSM_Action> StayActions = new List<HSM_Action>();
    public List<HSM_Action> ExitActions = new List<HSM_Action>();

    // links between each transition with its target state
    private Dictionary<HSM_Transition, HSM_State> Links;

    // current level in the hierarchy
    // higher hierarchy -> lower level
    public int Level;

    // parent state of the current one
    public HSM_State ParentState;

    public HSM_State(Enum name, int hierarchyLevel)
    {
        Name = name;
        Links = new Dictionary<HSM_Transition, HSM_State>();
        Level = hierarchyLevel;
    }

    public void AddParent(HSM_State parentState)
    {
        ParentState = parentState;
    }

    // the update for a standard state simply do nothing
    public virtual HSM_Result Update() { return null; }

    // the update down for a standard state
    public virtual void UpdateDown(HSM_State nextState, int level) { }

    // the update up for a standard state
    public virtual void UpdateUp(HSM_State lowerState) { }

    public void AddTransition(HSM_Transition transition, HSM_State state)
    {
        Links[transition] = state;
    }

    // return the difference in levels of the hierarchy between source and target states of a given transition
    // if 0 -> target state at same level than source state
    // if > 0 -> target state at higher level than source state
    // if < 0 -> target state at lower level than source state
    public int GetLevel(HSM_Transition transition)
    {
        return Level - Links[transition].Level;
    }

    public HSM_Transition VerifyTransitions()
    {
        foreach (HSM_Transition tran in Links.Keys)
        {
            if (tran.Condition()) return tran;
        }

        return null;
    }

    public HSM_State NextState(HSM_Transition transition)
    {
        return Links[transition];
    }

    public void Enter()
    {
        if (EnterActions != null) foreach (HSM_Action action in EnterActions) action();
    }

    public void Stay()
    {
        if (StayActions != null) foreach (HSM_Action action in StayActions) action();
    }

    public void Exit()
    {
        if (ExitActions != null) foreach (HSM_Action action in ExitActions) action();
    }
}
