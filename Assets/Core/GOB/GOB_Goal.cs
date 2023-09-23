using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GOB_Goal
{
    public float Value;

    public Scriptable_GOB_Goal Data;

    public void SetData(Scriptable_GOB_Goal scriptable)
    {
        Data = scriptable;
        Value = scriptable.Value;
    }
}
