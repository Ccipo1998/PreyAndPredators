using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FOV_Animal : MonoBehaviour
{
    [SerializeField]
    protected AI_Animal _AnimalAI;

    [SerializeField]
    protected MovementMechanic _MovementMechanic;

    [Header("Layers data")]
    [SerializeField]
    protected LayerMask _ResourcesLayerMask;
    [SerializeField]
    protected LayerMask _SimilarsLayerMask;

    [Header("FOV parameters")]
    [SerializeField]
    protected float _Length;
    [SerializeField]
    protected float _Angle;
    [SerializeField]
    protected bool _FixedUpdate = false;
    [SerializeField]
    protected float _FixedStep = .1f;

    [Header("FOV data")]
    [SerializeField]
    protected List<Resource> _Resources = new List<Resource>();
    [SerializeField]
    protected List<Animal> _Similars = new List<Animal>();

    public List<Resource> Resources { get => _Resources; }
    public List<Animal> Similars { get => _Similars; }

    protected float _min_dot;

    public float Length { get => _Length; }

    private void OnEnable()
    {
        // calculate min dot basing on angle
        Vector2 rotated = Quaternion.Euler(new Vector3(.0f, .0f, _Angle / 2.0f)) * _MovementMechanic.Direction;
        _min_dot = Vector2.Dot(_MovementMechanic.Direction, rotated);

        // start coroutine
        if (_FixedUpdate)
            StartCoroutine(FixedUpdateFOV());
        else
            StartCoroutine(UpdateFOV());

        DoOnEnable();
    }

    protected abstract void DoOnEnable();

    protected abstract void See();

    // update at each frame
    private IEnumerator UpdateFOV()
    {
        // for all animal life
        while (true)
        {
            See();

            yield return null;
        }
    }

    // update every fixed time
    private IEnumerator FixedUpdateFOV()
    {
        // for all animal life
        while (true)
        {
            See();

            yield return new WaitForSeconds(_FixedStep);
        }
    }

    private void Update()
    {
        #if DEBUG
            DrawViewCone();
        #endif
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
