using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected AI_Animal _AnimalAI;

    [SerializeField]
    protected FOV_Animal _FOV;

    [SerializeField]
    private List<Scriptable_GOB_Goal> _StartingNeeds = new List<Scriptable_GOB_Goal>();

    // dictionary of need id - value
    private Dictionary<int, GOB_Goal> _needs;

    public Dictionary<int, GOB_Goal> Needs { get => _needs; }

    // clone of needs for editor
    [SerializeField]
    private List<GOB_Goal> _Needs = new List<GOB_Goal>();

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

    private void SpaceRateUpdate()
    {
        GOB_Goal space = _needs[_SpaceResource.NeedId];

        // decrease rate is multiplied by the number of similars in FOV
        space.CurrentDecreaseRate = _FOV.Similars.Count * space.Data.DecreaseRate;
        // satisfy rate is applied only when the animal is isolated
        space.CurrentSatisfyRate = _FOV.Similars.Count == 0 ? space.Data.SatisfyRate : .0f;
    }

    private void SocialityRateUpdate()
    {
        GOB_Goal sociality = _needs[_SocialityResource.NeedId];

        // decrease rate is applied only when the animal is isolated
        sociality.CurrentDecreaseRate = _FOV.Similars.Count == 0 ? sociality.Data.DecreaseRate : .0f;
        // satisfy rate is multiplied by the number of similars in fov
        sociality.CurrentSatisfyRate = _FOV.Similars.Count * sociality.Data.SatisfyRate;
    }

    #region COROUTINES

    private IEnumerator NeedsUpdate()
    {
        // for all animal life
        while (true)
        {
            // update variable rates
            SpaceRateUpdate();
            SocialityRateUpdate();

            // update values
            GOB_Goal goal;
            foreach (int key in _needs.Keys)
            {
                goal = _needs[key];

                if (_needs[key].Data.ValueChangeWithTime)
                {
                    goal.Value = Mathf.Max(goal.Value - goal.Data.DecreaseRate * Time.deltaTime, goal.Data.MinValue);
                }
                else
                {
                    goal.Value = Mathf.Max(goal.Value - goal.CurrentDecreaseRate * Time.deltaTime, goal.Data.MinValue);
                    goal.Value = Mathf.Min(goal.Value + _needs[key].CurrentSatisfyRate * Time.deltaTime, goal.Data.MaxValue);

                }
            }

            yield return null;
        }
    }

    #endregion
}
