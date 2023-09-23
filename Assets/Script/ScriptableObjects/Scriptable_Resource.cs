using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Resource", order = 1)]
public class Scriptable_Resource : ScriptableObject
{
    public int NeedId;
    public string Name;
    public float Quantity;

    public float MinQuantity;
    public float MaxQuantity;

    public float GrowingRate;
}
