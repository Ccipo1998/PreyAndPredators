using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// defer function to trigger activation condition
// returns true when the transition can fire
[System.Serializable]
public delegate bool HSM_Condition();

// defer function to perform action
[System.Serializable]
public delegate void HSM_Action();

public class HSM_Transition
{
    // transition name -> for sanity
    public string Name;

    // the method to evaluate if the transition is ready to fire
    public HSM_Condition Condition;

    // a list of actions to perform when this transition fires
    private List<HSM_Action> Actions;

    public HSM_Transition(string name, HSM_Condition condition, List<HSM_Action> actions = null)
    {
        Name = name;
        Condition = condition;
        if (actions != null) Actions = actions;
    }

    public void AddAction(HSM_Action action)
    {
        if (Actions == null)
            Actions = new List<HSM_Action>();

        Actions.Add(action);
    }

    // call all actions
    public void Fire()
    {
        if (Actions == null) return;

        foreach (HSM_Action action in Actions)
        {
            action();
        }
    }
}
