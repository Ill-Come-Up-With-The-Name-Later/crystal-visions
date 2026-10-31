using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField]
    protected EntityStat _healthStat;
    public EntityStat HealthStat { get { return _healthStat; } }
    
    [SerializeField]
    protected EntityStat _damageStat;
    public EntityStat DamageStat { get { return _damageStat; } }

    [SerializeField]
    protected EntityStat _attackRangeStat;
    public EntityStat AttackRangeStat { get { return _attackRangeStat; } }
    
    [SerializeField]
    protected EntityStat _speedStat;
    public EntityStat SpeedStat { get { return _speedStat; } }
    
    [SerializeField]
    protected EntityStat _hitInvulnerabilityStat;
    public EntityStat HitInvulnerabilityStat { get { return _hitInvulnerabilityStat; } }
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    protected EntityHealth _health;
    
    [SerializeField]
    protected float _timeSinceLastDamage;
    
    public float TimeSinceLastDamage { get { return _timeSinceLastDamage; } set { _timeSinceLastDamage = value; } }
    
    [SerializeField]
    protected Rigidbody _rigidbody;
    
    public EntityHealth Health { get { return _health; } }
    
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
