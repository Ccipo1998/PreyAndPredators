using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BT_MonoBehavior : MonoBehaviour
{
    [SerializeField]
    protected bool _FixedUpdate = false;
    [SerializeField]
    protected float _FixedStep = .1f;

    protected BT_ITask _root;
    public BT_ITask Root { set { _root = value; } }

    protected Coroutine _coroutine;

    public void StartBT()
    {
      if (_FixedUpdate)
            _coroutine = StartCoroutine(FixedUpdateBT());
        else
            _coroutine = StartCoroutine(UpdateBT());
    }

    public void StopBT()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    // called at each frame
    private IEnumerator UpdateBT()
    {
        // coroutine must be stopped explicitly
        while (true)
        {
            _root.Run();

            yield return null;
        }
    }

    // called at fixed time steps
    private IEnumerator FixedUpdateBT()
    {
        // coroutine must be stopped explicitly
        while (true)
        {
            _root.Run();

            yield return new WaitForSeconds(_FixedStep);
        }
    }
}
