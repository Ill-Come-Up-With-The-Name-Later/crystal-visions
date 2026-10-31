using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Entity
{
    [SerializeField]
    private StateMachine _stateMachine;
    
    private Crystal _crystal;

    [SerializeField]
    private Vector3 _crystalPosition;
    
    public Vector3 CrystalPosition { get { return _crystalPosition; } }
    
    private Player _targetPlayer;
    
    [SerializeField]
    private Vector3 _playerPosition;
    
    public Vector3 PlayerPosition { get { return _playerPosition; } }

    [SerializeField]
    private Entity _target;
    
    public Entity Target { get => _target; set { _target = value; } }

    [Header("Attacks")]
    [SerializeField]
    private List<AttackAndCooldown> _attacks;
    
    [SerializeField]
    private float _globalAttackCooldown; // This applies to all of the enemy's attacks

    [Header("Enemy Stats")] 
    [SerializeField]
    private EntityStat _minRangeStat;
    public EntityStat MinRangeStat { get { return _minRangeStat; } }
    
    [SerializeField]
    private EntityStat _maxRangeStat;
    public EntityStat MaxRangeStat { get { return _maxRangeStat; } }

    public float GlobalAttackCooldown { get => _globalAttackCooldown; }

    private Dictionary<Attack, float> _attackCooldowns = new Dictionary<Attack, float>();
    
    new void Awake()
    {
        base.Awake();
        _stateMachine = GetComponent<StateMachine>();
        
        if(_stateMachine == null)
            Debug.LogError("No state machine found");
        
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");
    }

    void Start()
    {
        _damageStat.BonusValue = WaveManager.Instance.CurrentWave * 5;
        
        _targetPlayer = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();

        _health.OnDeath += Die;
        
        if(_crystal == null)
            Debug.LogError("No Crystal found");
        
        if(_crystal != null)
            _crystalPosition = new Vector3(_crystal.transform.position.x, 0, _crystal.transform.position.z);
        
        foreach(AttackAndCooldown attack in _attacks)
            _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    void OnDestroy()
    {
        _health.OnDeath -= Die;
    }

    void Update()
    {
        _playerPosition = _targetPlayer.transform.position;
        _timeSinceLastDamage += Time.deltaTime;
        
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;

        _globalAttackCooldown = Math.Max(0, _globalAttackCooldown - Time.deltaTime);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void UseRandomAttack()
    {
        AttackAndCooldown attack = _attacks[Random.Range(0, _attacks.Count)];
        
        UseAttack(attack);
    }

    public bool CanAttack()
    {
        return _globalAttackCooldown == 0;
    }
    
    public void UseAttack(AttackAndCooldown attack)
    {
        if (!CanUseAttack(attack.Attack))
            return;
        
        attack.Attack.PerformAttack(this);
        _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    public bool CanUseAttack(Attack attack)
    {
        _attackCooldowns.TryGetValue(attack, out var cooldown);
        return cooldown <= 0 && _globalAttackCooldown == 0;
    }
}
