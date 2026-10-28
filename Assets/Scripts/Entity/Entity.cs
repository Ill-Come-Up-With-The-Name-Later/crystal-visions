using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField]
    protected EntityStats _stats;

    [SerializeField]
    [Min(0)]
    protected float _damageBonus;

    public float DamageBonus
    {
        get { return _damageBonus; } 
        set { _damageBonus = value; }
    }
    
    [SerializeField]
    [Min(0)]
    protected float _multiplicativeDamageBonus;

    public float MultiplicativeDamageBonus
    {
        get { return _multiplicativeDamageBonus; } 
        set { _multiplicativeDamageBonus = value; }
    }
    
    public float Damage { get { return (_stats.damage + _damageBonus) * (1 + _multiplicativeDamageBonus); } }
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    protected EntityHealth _health;
    
    [SerializeField]
    protected float _timeSinceLastDamage;
    
    public float TimeSinceLastDamage { get { return _timeSinceLastDamage; } set { _timeSinceLastDamage = value; } }
    
    [SerializeField]
    protected Rigidbody _rigidbody;
    
    public EntityHealth Health { get { return _health; } }
    
    public EntityStats Stats { get { return _stats; } }
    
    public Rigidbody Rigidbody { get { return _rigidbody; } }
    
    protected void Awake()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");
    }

    protected void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }
}
