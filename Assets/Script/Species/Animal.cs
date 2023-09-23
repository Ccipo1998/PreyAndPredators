using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private List<Scriptable_GOB_Goal> _StartingNeeds;

    // dictionary of need id - value
    private Dictionary<int, GOB_Goal> _Needs;
    
    void OnEnable()
    {
        // init needs dictionary
        _Needs = new Dictionary<int, GOB_Goal>();
        for (int i = 0; i  < _StartingNeeds.Count; ++i)
        {
            GOB_Goal goal = new GOB_Goal();
            goal.SetData(_StartingNeeds[i]);
            _Needs.Add(goal.Data.Id, goal);
        }

        // start animal life coroutines
        StartCoroutine(NeedsUpdate());
    }

    #region COROUTINES

    public IEnumerator SatisfyNeed(int needId)
    {
        // only while the need is satisfied
        while (_Needs[needId].Value <= _Needs[needId].Data.MaxValue)
        {
            _Needs[needId].Value += _Needs[needId].Data.SatisfyRate * Time.deltaTime;

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
            foreach (int key in _Needs.Keys)
            {
                _Needs[key].Value -= _Needs[key].Data.DecreaseRate * Time.deltaTime;
            }

            yield return null;
        }
    }

    #endregion
}
