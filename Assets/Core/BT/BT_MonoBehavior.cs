using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public abstract class BT_MonoBehavior : MonoBehaviour
{
    [SerializeField]
    protected AI_Animal _AnimalAI;

    [SerializeField]
    protected bool _FixedUpdate = false;
    [SerializeField]
    protected float _FixedStep = .1f;
    [SerializeField]
    public int Status = -1;

    protected BT_ITask _root;
    public BT_ITask Root { set { _root = value; } }

    protected Coroutine _coroutine;

    // function to call just after stopping the BT
    protected abstract void OnStopBT();

    public void StartBT()
    {
        Status = -1;

        if (_FixedUpdate)
            _coroutine = StartCoroutine(FixedUpdateBT());
        else
            _coroutine = StartCoroutine(UpdateBT());
    }

    public void StopBT()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        // call function to do at stopping the BT
        OnStopBT();
    }

    // called at each frame
    private IEnumerator UpdateBT()
    {
        // coroutine must be stopped explicitly or due to success/fail of the BT
        while (Status == -1)
        {
            Status = _root.Run();

            yield return null;
        }
    }

    // called at fixed time steps
    private IEnumerator FixedUpdateBT()
    {
        // coroutine must be stopped explicitly or due to success/fail of the BT
        while (Status == -1)
        {
            Status = _root.Run();

            yield return new WaitForSeconds(_FixedStep);
        }
    }
}
