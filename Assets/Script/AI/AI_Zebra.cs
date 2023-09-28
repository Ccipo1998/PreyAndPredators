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

        // link states-transitions
        search.AddTransition(canSatisfy, satisfy);

        _hsm = new HSM("Zebra HSM", search, 0);
    }

    #region TRANSITIONS CONDITIONS

    // call the GOB -> if the selected goal has the relative object in fov, then can satisfy is true
    private bool CanSatisfy()
    {
        // get objects at minimum distance in fov
        
        List<GOB_Goal> goals = new List<GOB_Goal>();
        List<Resource> resources = new List<Resource>();
        List<float> times = new List<float>();

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

            // already inserted
            if (index != -1)
            {
                // nearer
                if (timeToReach < times[index])
                {
                    resources[index] = res;
                    times[index] = timeToReach;
                }

                continue;
            }
            
            // not inserted yet
            goals.Add(gg);
            resources.Add(res);
            times.Add(timeToReach);
        }

        // add special cases
        List<GOB_Goal> specialGoals = _Animal.Needs.Values.Where((v) => v.Data.CanBeAlwaysSatisfied).ToList();
        for (int i = 0; i < specialGoals.Count; ++i)
        {
            goals.Add(specialGoals[i]);
            resources.Add(null); // special goals dont have a resource associated
            times.Add(.0f); // specials goals are immediate
        }

        // perform gob

        GOB_Goal selected = GOB.ChooseConvenientBasicGoal(goals, times);

        // can satisfy
        if (selected != null)
        {
            // save the goal and the resource in BT_Satisfy

            return true;
        }

        // cannot satisfy
        return false;
    }

    #endregion
}
