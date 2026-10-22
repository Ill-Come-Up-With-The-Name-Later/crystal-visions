using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField]
    private EntityStats _stats;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private EntityHealth _health;
    
    public EntityHealth Health { get { return _health; } }
    
    public EntityStats Stats { get { return _stats; } }
    
    void Start()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
    }
}
