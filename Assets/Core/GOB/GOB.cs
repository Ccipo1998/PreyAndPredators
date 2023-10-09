using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public static class GOB
{
    /*
    // animal needs
    public static List<GOB_Goal> GetCurrentNeeds(Animal animal)
    {
        return new List<GOB_Goal> {
                new GOB_Goal(GoalName.Food, 100 - animal.Food),
                new GOB_Goal(GoalName.Water, 100 - animal.Water),
                new GOB_Goal(GoalName.Energy, 100 - animal.Energy)
            };
    }
    */

    //protected abstract float TargetGoalValue(GOB_Goal goal);

    // simple goal selection
    public static GOB_Goal ChoosePressingBasicGoal(List<GOB_Goal> needs)
    {
        GOB_Goal choosen = needs[0];
        for (int i = 1; i < needs.Count; i++)
        {
            if (needs[i].Value > choosen.Value)
                choosen = needs[i];
        }

        //choosen.Value = 100;
        return choosen;
    }

    // perform actual GOB
    public static int ChooseConvenientBasicGoal(List<GOB_Goal> needs, List<float> timeToReach)
    {
        // calculated values
        int chosen = -1;
        float finalDiscontentment = float.PositiveInfinity;

        // foreach goal i compute the discontentment after the completion of the action fulfilling the goal with an estimated timing
        for (int i = 0; i < needs.Count; i++)
        {
            //float target = TargetGoalValue(needs[0]);
            float discontentment = DiscontentmentAfterGoalSatisfied(needs, i, timeToReach[i]);

            if (discontentment < finalDiscontentment)
            {
                chosen = i;
                finalDiscontentment = discontentment;
                //chosen.Value = target;
            }
        }

        // target value saved inside the goal itself
        return chosen;
    }

    private static float DiscontentmentAfterGoalSatisfied(List<GOB_Goal> needs, int selectedGoal, float delayTime)
    {
        float disc = .0f;

        // calculate goal values after delay
        List<GOB_Goal> after = new List<GOB_Goal>();
        for (int i = 0; i < needs.Count; ++i)
        {
            after.Add(new GOB_Goal());
            after[i].Data = needs[i].Data;
            after[i].Value = needs[i].Value;
            after[i].CurrentDecreaseRate = needs[i].CurrentDecreaseRate;
        }

        float decreaseRate = .0f;
        for (int i = 0; i < after.Count; ++i)
        {
            if (after[i].Data.ValueChangeWithTime)
                decreaseRate = after[i].Data.DecreaseRate;
            else
                decreaseRate = after[i].CurrentDecreaseRate;

            after[i].Value -= decreaseRate * delayTime;
            after[i].Value = Mathf.Max(after[i].Data.MinValue, after[i].Value);
        }

        // time to satisfy selected goal
        float satisfyRate = after[selectedGoal].Data.ValueChangeWithTime ? after[selectedGoal].Data.SatisfyRate : after[selectedGoal].CurrentSatisfyRate;
        float time = (after[selectedGoal].Data.MaxValue - after[selectedGoal].Value) / satisfyRate;

        for (int i = 0; i < after.Count; ++i)
        {
            if (i == selectedGoal)
                disc += IncreasingDiscontentment(after[i], time);
            else
                disc += DecreasingDiscontentment(after[i], time);
        }

        return disc;
    }

    private static float IncreasingDiscontentment(GOB_Goal need, float time)
    {
        float satisfyRate = need.Data.ValueChangeWithTime ? need.Data.SatisfyRate : need.CurrentSatisfyRate;
        float disc = need.Data.MaxValue - need.Value - (satisfyRate * time);
        return disc * disc;
    }

    private static float DecreasingDiscontentment(GOB_Goal need, float time)
    {
        float decreaseRate = need.Data.ValueChangeWithTime ? need.Data.DecreaseRate : need.CurrentDecreaseRate;
        float disc = need.Data.MaxValue - need.Value + (decreaseRate * time);
        disc = Mathf.Max(need.Data.MinValue, disc);
        return disc * disc;
    }

    /*
    private static float DiscontentmentAfterGoalSatisfied(GameObject animalObject, GoalName name, ref float target)
    {
        // 1. timing estimation to arrive to spot (0 for sleep)
        GameObject spotObject = null;
        ResourceSpot nearerSpot = null;
        float distance = 0f;
        float timing;
        switch (name)
        {
            case GoalName.Food:
                spotObject = animalObject.GetComponent<AnimalFOV>().GetNearerFoodSpot();
                if (spotObject != null)
                {
                    nearerSpot = spotObject.GetComponent<ResourceSpot>();
                    distance = Vector3.Distance(animalObject.transform.position, nearerSpot.transform.position);
                }
                else
                {
                    // high value for unkown resource position
                    distance = 10f;
                }

                break;

            case GoalName.Water:

                spotObject = animalObject.GetComponent<AnimalFOV>().GetNearerWaterSpot();
                if (spotObject != null)
                {
                    nearerSpot = spotObject.GetComponent<ResourceSpot>();
                    distance = Vector3.Distance(animalObject.transform.position, nearerSpot.transform.position);
                }
                else
                {
                    // high value for unkown resource position
                    distance = 10f;
                }

                break;

            case GoalName.Energy:
                // nothing (distance = 0)
                break;
        }

        // medium timing using medium (standard) speed
        timing = distance / animalObject.GetComponent<Move>().StandardSpeed;

        // 2. extimation of new values of goals
        float reaction = .0f;
        if (animalObject.tag == "Zebra")
            reaction = animalObject.GetComponent<ZebraBehavior>().ReactionTime;
        else // Lion
            reaction = animalObject.GetComponent<LionBehavior>().ReactionTime;

        float afterFood = (100 - animalObject.GetComponent<Animal>().Food) + timing / animalObject.GetComponent<Animal>().FoodDecreaseRate;
        float afterWater = (100 - animalObject.GetComponent<Animal>().Water) + timing / animalObject.GetComponent<Animal>().WaterDecreaseRate;
        // effort = time effort + movement effort (using standard speed)
        float afterEnergy = 100 - animalObject.GetComponent<Animal>().Energy;
        afterEnergy += timing / animalObject.GetComponent<AnimalEffort>().TimeRate; // * 1
        afterEnergy += timing / reaction * animalObject.GetComponent<AnimalEffort>().EffortValue;

        // 3. goals values extimations after performing satisfy need action with different durations + choose best discontentment
        float finalFood = afterFood;
        float finalWater = afterWater;
        float finalEnergy = afterEnergy;

        float finalDiscontentment = float.PositiveInfinity;
        float resourceTarget = 0f;
        switch (name)
        {
            case GoalName.Food:

                // try 5, 10, 15 and 20 seconds
                for (int t = 5; t < 21; t += 5)
                {
                    finalFood = afterFood - (t / reaction * animalObject.GetComponent<AnimalSatisfyNeed>().EatValue);
                    if (finalFood < 0)
                        finalFood = 0;
                    finalWater = afterWater + t * (1 / animalObject.GetComponent<Animal>().WaterDecreaseRate);
                    if (finalWater < 0)
                        finalWater = 0;
                    finalEnergy = afterEnergy;
                    if (finalEnergy < 0)
                        finalEnergy = 0;

                    // food and water have bigger weight compared to energy
                    float discontentment = Discontentment(finalFood >= 100 ? finalFood * 2 : finalFood) + Discontentment(finalWater >= 100 ? finalWater * 2 : finalWater) + Discontentment(finalEnergy);
                    if (discontentment < finalDiscontentment)
                    {
                        finalDiscontentment = discontentment;
                        resourceTarget = 100 - finalFood;
                    }
                }

                break;

            case GoalName.Water:

                // try 5, 10, 15 and 20 seconds
                for (int t = 5; t < 21; t += 5)
                {
                    finalFood = afterFood + t * (1 / animalObject.GetComponent<Animal>().FoodDecreaseRate);
                    if (finalFood < 0)
                        finalFood = 0;
                    finalWater = afterWater - (t / reaction * animalObject.GetComponent<AnimalSatisfyNeed>().DrinkValue);
                    if (finalWater < 0)
                        finalWater = 0;
                    finalEnergy = afterEnergy;
                    if (finalEnergy < 0)
                        finalEnergy = 0;

                    // food and water have bigger weight compared to energy
                    float discontentment = Discontentment(finalFood >= 100 ? finalFood * 2 : finalFood) + Discontentment(finalWater >= 100 ? finalWater * 2 : finalWater) + Discontentment(finalEnergy);
                    if (discontentment < finalDiscontentment)
                    {
                        finalDiscontentment = discontentment;
                        resourceTarget = 100 - finalWater;
                    }
                }

                break;

            case GoalName.Energy:

                // try 5, 10, 15 and 20 seconds
                for (int t = 5; t < 21; t += 5)
                {
                    finalFood = afterFood + t * (1 / animalObject.GetComponent<Animal>().FoodDecreaseRate);
                    if (finalFood < 0)
                        finalFood = 0;
                    finalWater = afterWater + t * (1 / animalObject.GetComponent<Animal>().WaterDecreaseRate);
                    if (finalWater < 0)
                        finalWater = 0;
                    finalEnergy = afterEnergy - (t / reaction * animalObject.GetComponent<AnimalSatisfyNeed>().SleepValue);
                    if (finalEnergy < 0)
                        finalEnergy = 0;

                    // food and water have bigger weight compared to energy
                    float discontentment = Discontentment(finalFood >= 100 ? finalFood * 2 : finalFood) + Discontentment(finalWater >= 100 ? finalWater * 2 : finalWater) + Discontentment(finalEnergy);
                    if (discontentment < finalDiscontentment)
                    {
                        finalDiscontentment = discontentment;
                        resourceTarget = 100 - finalEnergy;
                    }
                }

                break;
        }

        target = resourceTarget;
        return finalDiscontentment;
    }
    */

    /*
    // change the next goal of the Zebra
    public static GOB_Goal SelectNextGoal(GameObject animalObject)
    {
        // compute next basic goal to survive
        return ChooseConvenientBasicGoal(animalObject);
    }
    */
}
