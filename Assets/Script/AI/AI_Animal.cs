using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Animal : MonoBehaviour
{
    [SerializeField]
    protected float _ReactionTime;

    // number of behavior trees associated to the AI -> for initialization
    [SerializeField]
    protected int _BtsNumber;

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
