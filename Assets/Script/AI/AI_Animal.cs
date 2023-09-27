using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Animal : MonoBehaviour
{
    [SerializeField]
    protected float _ReactionTime;

    protected HSM _hsm;

    public abstract void BT_Initialized();

    protected IEnumerator UpdateHSM()
    {
        // for all zebra's life
        while (true)
        {
            _hsm.Update();

            yield return new WaitForSeconds(_ReactionTime);
        }
    }
}
