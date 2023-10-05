using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// all zebra's ai mechanics are handled here
public class AI_Zebra : AI_Animal
{
    [Header("Animal data")]
    [SerializeField]
    private Animal _Animal;

    [Header("Mechanics")]
    [SerializeField]
    private MovementMechanic _MovementMechanic;
    [SerializeField]
    private FOV_Herbivore _FOV;

    [Header("GOB parameters")]
    [SerializeField]
    private GOB_Goal _CurrentGoal;
    [SerializeField]
    private float _DefaultTimeToReach;

    [Header("BTs")]
    [SerializeField]
    private BT_Search _BTSearch;
    [SerializeField]
    private BT_Satisfy _BTSatisfy;

    private float timeToIsolate;

    private GameObject _gob_target;

    #region BTs INITS

    public override void MonoBehaviourReady()
    {
        --_MonobehavioursNumber;

        if (_MonobehavioursNumber == 0)
            StartCoroutine(UpdateHSM());
    }

    #endregion

    private void OnEnable()
    {
        // compute time to isolate
        timeToIsolate = _FOV.Length / _MovementMechanic.WalkSpeed;

        // hard-coded zebra HSM

        // states
        HSM_State search = new HSM_State("Search", 0);
        search.EnterActions.Add(_BTSearch.StartBT);
        search.EnterActions.Add(PerformGOB);
        search.StayActions.Add(PerformGOB);
        search.ExitActions.Add(_BTSearch.StopBT);

        HSM_State satisfy = new HSM_State("Satisfy", 0);
        satisfy.EnterActions.Add(_BTSatisfy.StartBT);
        satisfy.ExitActions.Add(_BTSatisfy.StopBT);

        // transitions
        HSM_Transition canSatisfy = new HSM_Transition("Can satisfy", NeedResource);
        HSM_Transition satisfied = new HSM_Transition("Satisfied", Satisfied);

        // link states-transitions
        search.AddTransition(canSatisfy, satisfy);
        satisfy.AddTransition(satisfied, search);

        _hsm = new HSM("Zebra HSM", search, 0);
    }

    #region STATES ACTIONS

    // call GOB
    private void PerformGOB()
    {
        // get goals with default data
        List<GOB_Goal> goals = new List<GOB_Goal>();
        List<GameObject> targets = new List<GameObject>();
        List<float> times = new List<float>();

        float timeToReach = _DefaultTimeToReach;

        foreach (GOB_Goal goal in _Animal.Needs.Values)
        {
            goals.Add(goal);
            targets.Add(null);

            if (goal.Data.CanBeAlwaysSatisfied)
                times.Add(.0f);
            else
                times.Add(timeToReach);
        }

        // resources
        foreach (Resource res in _FOV.Resources)
        {
            // check goal
            GOB_Goal gg = _Animal.Needs[res.Data.NeedId];
            int index = goals.IndexOf(gg);
            timeToReach = (res.transform.position - transform.position).magnitude / _MovementMechanic.WalkSpeed; // time to reach current resource

            if (timeToReach < times[index])
            {
                // nearer
                targets[index] = res.gameObject;
                times[index] = timeToReach;
            }
        }

        // similar
        foreach (Animal anim in _FOV.Similars)
        {
            // check goal
            GOB_Goal gg = _Animal.Needs[anim.SocialityResource.NeedId];
            int index = goals.IndexOf(gg);
            timeToReach = (anim.transform.position - transform.position).magnitude / _MovementMechanic.WalkSpeed; // time to reach current similar

            if (timeToReach < times[index])
            {
                // nearer
                targets[index] = anim.gameObject;
                times[index] = timeToReach;
            }
        }

        // space
        GOB_Goal space = _Animal.Needs[_Animal.SpaceResource.NeedId];
        int i = goals.IndexOf(space);
        times[i] = timeToIsolate - timeToReach;

        // GOB
        int selectedIndex = GOB.ChooseConvenientBasicGoal(goals, times); // always != -1

        _CurrentGoal = goals[selectedIndex];
        _gob_target = targets[selectedIndex];
    }

    #endregion

    #region TRANSITIONS CONDITIONS

    // check if current goal can be satisfied with a resource in FOV
    private bool NeedResource()
    {
        if (_CurrentGoal == null || _CurrentGoal.Data == null)
            return false;

        if (_CurrentGoal.Data.CanBeAlwaysSatisfied || _gob_target != null)
        {
            _BTSatisfy.Goal = _CurrentGoal;
            _BTSatisfy.Target = _gob_target;

            return true;
        }

        return false;
    }

    // check whenever the current goal is been satisfied
    private bool Satisfied()
    {
        return _BTSatisfy.Status == 1;
    }

    #endregion
}
