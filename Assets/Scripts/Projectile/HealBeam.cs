using System.Collections;
using UnityEngine;

public class HealBeam : MonoBehaviour
{
    [Header("Heal Beam Settings")]
    [SerializeField]
    private Entity _source;
    
    [SerializeField]
    private Entity _target;

    [Min(0f)]
    private float _healAmount;
    
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
        _beamRenderer.enabled = true;
        _beamRenderer.positionCount = 2;
        
        _beamRenderer.SetPosition(0, _source.transform.position);
        _beamRenderer.SetPosition(1, _target.transform.position);
        
        _target.Health.Heal(_healAmount);
        StartCoroutine(TerminateBeam());
    }

    IEnumerator TerminateBeam()
    {
        yield return new WaitForSeconds(0.25f);
        
        _beamRenderer.enabled = false;
        Destroy(gameObject);
    }
}