using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource : MonoBehaviour
{
    public float Quantity;

    public Scriptable_Resource Data;

    public void SetData(Scriptable_Resource scriptable)
    {
        Data = scriptable;
        Quantity = scriptable.Quantity;
    }

    private void OnEnable()
    {
        // init quantity
        Quantity = Data.Quantity;

        // start resource life coroutines
        StartCoroutine(QuantityUpdate());
    }

    #region COROUTINES

    private IEnumerator QuantityUpdate()
    {
        // for all resource life
        while (true)
        {
            Quantity += Data.GrowingRate * Time.deltaTime;

            Quantity = Mathf.Min(Data.MaxQuantity, Quantity);

            yield return null;
        }
    }

    #endregion
}
