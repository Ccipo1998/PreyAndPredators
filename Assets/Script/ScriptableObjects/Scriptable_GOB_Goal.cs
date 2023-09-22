using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GOB_Goal", order = 1)]
public class Scriptable_GOB_Goal : ScriptableObject
{
    public string Name;
    public float Value;

    public float SatisfyRate, DecreaseRate;

    public float MinValue, MaxValue;
}
