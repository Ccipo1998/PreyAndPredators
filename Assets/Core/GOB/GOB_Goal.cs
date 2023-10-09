using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOB_Goal
{
    public float Value;

    private float _current_decrease_rate;
    private float _current_satisfy_rate;

    public float CurrentDecreaseRate { get => _current_decrease_rate; set => _current_decrease_rate = value; }
    public float CurrentSatisfyRate { get => _current_satisfy_rate; set => _current_satisfy_rate = value; }

    public Scriptable_GOB_Goal Data;

    public void SetData(Scriptable_GOB_Goal scriptable)
    {
        Data = scriptable;
        Value = scriptable.Value;
        _current_decrease_rate = .0f;
    }
}
