using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Header("Attacks")]
    [SerializeField]
    private List<AttackAndCooldown> _attacks;
    
    private Dictionary<Attack, float> _attackCooldowns = new Dictionary<Attack, float>();
    
    void Awake()
    {
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
        _targetPlayer = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
        
        if(_crystal == null)
            Debug.LogError("No Crystal found");
        
        _crystalPosition = new Vector3(_crystal.transform.position.x, 0, _crystal.transform.position.z);
        
        foreach(AttackAndCooldown attack in _attacks)
            _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    void Update()
    {
        _playerPosition = _targetPlayer.transform.position;
        
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;
    }

    public void UseRandomAttack()
    {
        AttackAndCooldown attack = _attacks[Random.Range(0, _attacks.Count)];
        
        UseAttack(attack);
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
        return cooldown <= 0;
    }
}
