using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected AI_Animal _AnimalAI;

    [SerializeField]
    private List<Scriptable_GOB_Goal> _StartingNeeds;

    // dictionary of need id - value
    private Dictionary<int, GOB_Goal> _needs;

    public Dictionary<int, GOB_Goal> Needs { get => _needs; }

    // clone of needs for editor
    [SerializeField]
    private List<GOB_Goal> _Needs;

    [SerializeField]
    private Scriptable_Resource _SocialityResource;
    [SerializeField]
    private Scriptable_Resource _SpaceResource;

    public Scriptable_Resource SocialityResource { get => _SocialityResource; }
    public Scriptable_Resource SpaceResource { get => _SpaceResource; }

    void OnEnable()
    {
        // init needs dictionary
        _needs = new Dictionary<int, GOB_Goal>();
        for (int i = 0; i  < _StartingNeeds.Count; ++i)
        {
            GOB_Goal goal = new GOB_Goal();
            goal.SetData(_StartingNeeds[i]);
            _needs.Add(goal.Data.Id, goal);

            _Needs.Add(goal);
        }

        // start animal life coroutines
        StartCoroutine(NeedsUpdate());

        // at end of initialization, always notify that
        _AnimalAI.MonoBehaviourReady();
    }

    #region COROUTINES

    private IEnumerator NeedsUpdate()
    {
        // for all animal life
        while (true)
        {
            foreach (int key in _needs.Keys)
            {
                _needs[key].Value -= _needs[key].Data.DecreaseRate * Time.deltaTime;
            }

            yield return null;
        }
    }

    #endregion
}
