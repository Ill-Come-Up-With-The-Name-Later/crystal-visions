using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField]
    protected EntityStats _stats;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    protected EntityHealth _health;
    
    [SerializeField]
    protected Rigidbody _rigidbody;
    
    public EntityHealth Health { get { return _health; } }
    
    public EntityStats Stats { get { return _stats; } }
    
    public Rigidbody Rigidbody { get { return _rigidbody; } }
    
    void Awake()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");
    }
}
