using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class MovementMechanic : MonoBehaviour
{
    [Header("Movement parameters")]

    [SerializeField]
    private float _RunSpeed;
    [SerializeField]
    private float _WalkSpeed;
    [SerializeField]
    private float _Acceleration;
    [SerializeField]
    private float _Deceleration;
    [SerializeField]
    private float _StopDistance;

    [Header("Movement data")]

    [SerializeField]
    private Vector2 _Direction;
    [SerializeField]
    private float _Speed;

    private Coroutine _speed_update;
    private Vector2? _target;

    public Vector2 Direction { get => _Direction; }
    public float WalkSpeed { get => _WalkSpeed; }
    public float StopDistance { get => _StopDistance; }

    public void RandomWalk()
    {
        // stop current speed update (it could be running)
        if (_speed_update != null)
            StopCoroutine(_speed_update);

        // set random direction
        _Direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        // avoid vector zero case
        if (_Direction.magnitude == .0f)
            _Direction.x = 1.0f;
        _Direction.Normalize();

        // match walk speed
        _speed_update = StartCoroutine(ToWalkSpeed());
    }

    public void GoTo(Vector2 position)
    {
        // stop current speed update (it could be running)
        if (_speed_update != null)
            StopCoroutine(_speed_update);

        // set direction
        _Direction = (position - (Vector2)transform.position).normalized;

        // save target
        _target = position;

        // match walk speed
        _speed_update = StartCoroutine(ToWalkSpeed());

        // check arriving
        StartCoroutine(StopAtArriving());
    }

    public void Stop()
    {
        // stop current speed update (it could be running)
        if (_speed_update != null)
            StopCoroutine(_speed_update);

        _target = null;

        // match 0 speed
        _speed_update = StartCoroutine(ToStopSpeed());
    }

    // position updated at each frame
    private void Update()
    {
        transform.position += new Vector3(_Direction.x, _Direction.y, .0f) * _Speed * Time.deltaTime;
    }

    #region COROUTINES

    // at each frame, it updates the speed to match walk speed
    private IEnumerator ToWalkSpeed()
    {
        // deceleration needed
        if (_Speed > _WalkSpeed)
        {
            // decelerate
            while (_Speed > _WalkSpeed)
            {
                _Speed += _Deceleration * Time.deltaTime;

                yield return null;
            }
        }
        // acceleration needed
        else
        {
            // accelerate
            while (_Speed < _WalkSpeed)
            {
                _Speed += _Acceleration * Time.deltaTime;

                yield return null;
            }
        }

        // insure correct speed
        _Speed = _WalkSpeed;
    }

    private IEnumerator ToStopSpeed()
    {
        // deceleration needed
        // decelerate
        while (_Speed > .0f)
        {
            _Speed += _Deceleration * Time.deltaTime;

            yield return null;
        }

        // insure zero speed
        _Speed = .0f;
        _target = null;
    }

    private IEnumerator StopAtArriving()
    {
        while (((Vector2)_target - (Vector2)transform.position).magnitude > _StopDistance)
        {
            yield return null;
        }

        if (_speed_update != null)
            StopCoroutine(_speed_update);
        _speed_update = StartCoroutine(ToStopSpeed());
    }

    #endregion
}
