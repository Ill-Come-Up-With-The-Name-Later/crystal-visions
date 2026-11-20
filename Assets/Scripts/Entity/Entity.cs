using System;
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

    [SerializeField]
    protected EntityStat _knockbackResistanceStat;
    public EntityStat KnockbackResistanceStat { get { return _knockbackResistanceStat; } }
    
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
    
    protected virtual void Awake()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");

        _health.OnDamageTaken += TakeKnockback;
    }

    protected virtual void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
    }

    protected virtual void OnDestroy()
    {
        _health.OnDamageTaken -= TakeKnockback;
    }

    void TakeKnockback(float damage)
    {
        _rigidbody.linearVelocity += (-transform.forward + new Vector3(0, 0.75f, 0)) 
                                     * (1 - _knockbackResistanceStat.Value) * (damage / 5);
    }
}
