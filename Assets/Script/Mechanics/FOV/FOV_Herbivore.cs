using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FOV_Herbivore : FOV_Animal
{
    [SerializeField]
    protected LayerMask _ResourcesLayerMask;
    [SerializeField]
    protected LayerMask _PredatorsLayerMask;
    [SerializeField]
    protected LayerMask _SimilarsLayerMask;

    [Header("FOV data")]
    [SerializeField]
    private List<Resource> _Resources;
    [SerializeField]
    private List<Animal> _Predators;
    [SerializeField]
    private List<Animal> _Similars;

    public List<Resource> Resources { get => _Resources; }
    public List<Animal> Predators { get => _Predators; }
    public List<Animal> Similars { get => _Similars; }

    protected override void DoOnEnable()
    {
        // init seen list
        _Resources = new List<Resource>();
        _Predators = new List<Animal>();
        _Similars = new List<Animal>();

        // at end of initialization, always notify that
        _AnimalAI.MonoBehaviourReady();
    }

    protected override void See()
    {
        // clear old seen
        _Resources.Clear();
        _Predators.Clear();
        _Similars.Clear();

        // get nears

        // check basing on view cone
        List<Collider2D> resources = Physics2D.OverlapCircleAll(transform.position, _Length, _ResourcesLayerMask).ToList();
        foreach (Collider2D collider in resources)
        {
            float dot = Vector2.Dot((collider.transform.position - transform.position).normalized, _MovementMechanic.Direction);

            // collider inside view cone
            if (dot >= _min_dot)
            {
                _Resources.Add(collider.GetComponent<Resource>());
            }
        }

        // check basing on view cone
        List<Collider2D> predators = Physics2D.OverlapCircleAll(transform.position, _Length, _PredatorsLayerMask).ToList();
        foreach (Collider2D collider in predators)
        {
            float dot = Vector2.Dot((collider.transform.position - transform.position).normalized, _MovementMechanic.Direction);

            // collider inside view cone
            if (dot >= _min_dot)
            {
                _Predators.Add(collider.GetComponent<Animal>());
            }
        }

        // check basing on view cone
        List<Collider2D> similars = Physics2D.OverlapCircleAll(transform.position, _Length, _SimilarsLayerMask).ToList();
        foreach (Collider2D collider in similars)
        {
            float dot = Vector2.Dot((collider.transform.position - transform.position).normalized, _MovementMechanic.Direction);

            // collider inside view cone
            if (dot >= _min_dot)
            {
                _Similars.Add(collider.GetComponent<Animal>());
            }
        }
    }
}
