using System.Collections;
using System.Collections.Generic;
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

    [Header("Movement data")]

    [SerializeField]
    private Vector2 _Direction;
    [SerializeField]
    private float _Speed;

    private Coroutine _speed_update;

    public Vector2 Direction { get => _Direction; }

    public int RandomWalk()
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
        StartCoroutine(ToWalkSpeed());

        return 1;
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

    #endregion
}
