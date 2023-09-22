using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDataController : MonoBehaviour
{
    [SerializeField]
    private GameObject _AnimalNeedsPanel;

    [SerializeField]
    private GameObject _NeedPanel;

    private List<NeedDataController> _controllers;
    private List<GOB_Goal> _goals;

    public void Populate(List<GOB_Goal> needs)
    {
        // save references
        _goals = needs;
        _controllers = new List<NeedDataController>();

        for (int i = 0; i < needs.Count; ++i)
        {
            GameObject newGO = Instantiate(_NeedPanel, _AnimalNeedsPanel.transform);
            _controllers.Add(newGO.GetComponent<NeedDataController>());
            _controllers[i]._NeedName.text = _goals[i].Data.Name;
            _controllers[i]._NeedValue.text = _goals[i].Value.ToString("0.00");
            // TODO: icon
        }
    }

    void Update()
    {
        // temp -> this game object will be enabled only after populate
        if (_goals == null || _controllers == null)
            return;

        for (int i = 0; i < _goals.Count; ++i)
        {
            _controllers[i]._NeedName.text = _goals[i].Data.Name;
            _controllers[i]._NeedValue.text = _goals[i].Value.ToString("0.00");
        }
    }
}
