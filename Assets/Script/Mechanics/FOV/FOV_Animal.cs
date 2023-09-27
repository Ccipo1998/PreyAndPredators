using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FOV_Animal : MonoBehaviour
{
    [SerializeField]
    private MovementMechanic _MovementMechanic;

    [Header("FOV parameters")]
    [SerializeField]
    private float _Length;
    [SerializeField]
    private float _Angle;

    [Header("FOV data")]
    [SerializeField]
    public List<Collider2D> _Seen;

    [SerializeField]
    private float _min_dot;

    private void OnEnable()
    {
        // init seen list
        _Seen = new List<Collider2D>();

        // calculate min dot basing on angle
        Vector2 rotated = Quaternion.Euler(new Vector3(.0f, .0f, _Angle / 2.0f)) * _MovementMechanic.Direction;
        _min_dot = Vector2.Dot(_MovementMechanic.Direction, rotated);
    }

    private void See()
    {
        // clear old seen
        _Seen.Clear();

        // check around
        Collider2D[] near = Physics2D.OverlapCircleAll(transform.position, _Length);

        // check basing on view cone
        foreach (Collider2D collider in near)
        {
            float dot = Vector2.Dot((collider.transform.position - transform.position).normalized, _MovementMechanic.Direction);
            
            // collider inside view cone
            if (dot >=  _min_dot)
            {
                _Seen.Add(collider);
            }
        }
    }

    private void FixedUpdate()
    {
        #if DEBUG
            DrawViewCone();
        #endif

        See();
    }

    #region DEBUG

    private void DrawViewCone()
    {
        // calculate edges
        Vector3 left = Quaternion.Euler(new Vector3(.0f, .0f, _Angle / 2.0f)) * _MovementMechanic.Direction;
        Vector3 right = Quaternion.Euler(new Vector3(.0f, .0f, -_Angle / 2.0f)) * _MovementMechanic.Direction;

        Debug.DrawLine(transform.position, transform.position + (left * _Length));
        Debug.DrawLine(transform.position, transform.position + (right * _Length));
    }

    #endregion
}
