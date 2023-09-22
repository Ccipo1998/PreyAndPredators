using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private List<Scriptable_GOB_Goal> _StartingNeeds;

    // list of needs (or goals)
    // convention: index = goal id
    private List<GOB_Goal> _needs;

    void OnEnable()
    {
        // copy starting needs
        // assert convention: index = goal id
        _needs = new List<GOB_Goal>();
        for (int i = 0; i  < _StartingNeeds.Count; ++i)
        {
            GOB_Goal goal = new GOB_Goal();
            goal.Id = i;
            goal.Value = _StartingNeeds[i].Value;
            goal.Data = _StartingNeeds[i];
            _needs.Add(goal);
        }

        // start animal life coroutines
        StartCoroutine(NeedsUpdate());
    }

    #region COROUTINES

    public IEnumerator SatisfyNeed(int needId)
    {
        // only while the need is satisfied
        while (_needs[needId].Value <= _needs[needId].Data.MaxValue)
        {
            _needs[needId].Value += _needs[needId].Data.SatisfyRate * Time.deltaTime;

            yield return null;
        }
    }

    /*
    public IEnumerator DecreaseNeed(int needId)
    {

    }
    */

    private IEnumerator NeedsUpdate()
    {
        // for all animal life
        while (true)
        {
            for (int i = 0; i < _needs.Count; ++i)
            {
                _needs[i].Value -= _needs[i].Data.DecreaseRate * Time.deltaTime;
            }

            yield return null;
        }
    }

    #endregion
}
