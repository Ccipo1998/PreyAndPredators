using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GOB_Goal", order = 1)]
public class GOB_Goal : ScriptableObject
{
    // TODO: this should be not editable in editor
    private int Id;

    public string Name;
    public float Value;

    public float SatisfyRate, DecreaseRate;

    public float MinValue, MaxValue;

    public void SetId(int id)
    {
        Id = id;
    }
}
