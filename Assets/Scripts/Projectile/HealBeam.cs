using System.Collections;
using UnityEngine;

public class HealBeam : MonoBehaviour
{
    [Header("Heal Beam Settings")]
    [SerializeField]
    private Entity _source;
    
    public Entity Source { get { return _source; } set { _source = value; } }
    
    [SerializeField]
    private Entity _target;
    
    public Entity Target { get { return _target; } set { _target = value; } }

    [Min(0f)]
    private float _healAmount;
    
    public float HealAmount { get { return _healAmount; } set { _healAmount = value; } }
    
    [Header("Debug (Do not edit)")]
    private LineRenderer _beamRenderer;

    void Awake()
    {
        _beamRenderer = GetComponent<LineRenderer>();
        
        if(_beamRenderer == null)
            Debug.LogWarning("No line renderer found");
    }

    public void Heal()
    {
        if (_source != null)
            transform.position = _source.transform.position;
        
        _beamRenderer.enabled = true;
        _beamRenderer.positionCount = 2;
        
        _beamRenderer.SetPosition(0, transform.position);
        _beamRenderer.SetPosition(1, _target.transform.position);
        
        _target.Health.Heal(_healAmount, EntityHealth.HealReason.Ability);
        StartCoroutine(TerminateBeam());
    }

    IEnumerator TerminateBeam()
    {
        yield return new WaitForSeconds(0.25f);
        
        _beamRenderer.enabled = false;
        Destroy(gameObject);
    }
}