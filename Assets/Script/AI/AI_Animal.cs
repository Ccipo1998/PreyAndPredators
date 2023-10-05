using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Animal : MonoBehaviour
{
    [SerializeField]
    protected float _ReactionTime;

    // number of scripts attached to the AI -> for initialization
    [SerializeField]
    protected int _MonobehavioursNumber;

    protected HSM _hsm;

    public abstract void MonoBehaviourReady();

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
