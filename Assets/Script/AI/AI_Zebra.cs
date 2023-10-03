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
    private FOV_Animal _FovAnimal;

    [Header("GOB parameters")]
    [SerializeField]
    private float _DefaultTimeToReach;

    [Header("BTs")]
    [SerializeField]
    private BT_Search _BTSearch;
    [SerializeField]
    private BT_Satisfy _BTSatisfy;

    #region BTs INITS

    public override void BT_Initialized()
    {
        --_BTsNumber;

        if (_BTsNumber == 0)
            StartCoroutine(UpdateHSM());
    }

    #endregion

    private void OnEnable()
    {
        // hard-coded zebra HSM

        // states
        HSM_State search = new HSM_State("Search", 0);
        search.EnterActions.Add(_BTSearch.StartBT);
        search.ExitActions.Add(_BTSearch.StopBT);

        HSM_State satisfy = new HSM_State("Satisfy", 0);
        satisfy.EnterActions.Add(_BTSatisfy.StartBT);
        satisfy.ExitActions.Add(_BTSatisfy.StopBT);

        // transitions
        HSM_Transition canSatisfy = new HSM_Transition("Can satisfy", CanSatisfy);
        HSM_Transition satisfied = new HSM_Transition("Satisfied", Satisfied);

        // link states-transitions
        search.AddTransition(canSatisfy, satisfy);
        satisfy.AddTransition(satisfied, search);

        _hsm = new HSM("Zebra HSM", search, 0);
    }

    #region TRANSITIONS CONDITIONS

    // call the GOB -> if the selected goal has the relative object in fov, then can satisfy is true
    private bool CanSatisfy()
    {
        // get objects at minimum distance in fov
        
        // get goals with default data
        List<GOB_Goal> goals = new List<GOB_Goal>();
        List<Resource> resources = new List<Resource>();
        List<float> times = new List<float>();

        foreach (GOB_Goal goal in _Animal.Needs.Values)
        {
            goals.Add(goal);
            resources.Add(null);

            if (goal.Data.CanBeAlwaysSatisfied)
                times.Add(.0f);
            else
                times.Add(_DefaultTimeToReach);
        }

        // update goals' distances and resources basing on fov data
        foreach (Collider2D collider in _FovAnimal.Seen)
        {
            // check if it is a resource
            Resource res = collider.gameObject.GetComponent<Resource>();
            if (res == null)
                continue;

            // check goal
            GOB_Goal gg = _Animal.Needs[res.Data.NeedId];
            int index = goals.IndexOf(gg);
            float timeToReach = (collider.transform.position - transform.position).magnitude / _MovementMechanic.WalkSpeed; // time to reach current resource

            if (timeToReach < times[index])
            {
                // nearer
                resources[index] = res;
                times[index] = timeToReach;
            }
        }

        // perform gob

        int selectedIndex = GOB.ChooseConvenientBasicGoal(goals, times); // always != -1

        // resource available or can always satisfy -> can satisfy
        if (resources[selectedIndex] != null || goals[selectedIndex].Data.CanBeAlwaysSatisfied)
        {
            // save the goal and the resource in BT_Satisfy
            _BTSatisfy.Goal = goals[selectedIndex];
            _BTSatisfy.Resource = resources[selectedIndex];

            return true;
        }

        // cannot satisfy
        return false;
    }

    // check whenever the current goal is been satisfied
    private bool Satisfied()
    {
        return _BTSatisfy.Status == 1;
    }

    #endregion
}
