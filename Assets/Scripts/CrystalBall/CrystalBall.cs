using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum MouseButton
{
    Left,
    Right,
    Middle
}

[Serializable]
struct AttackMode
{
    [Header("Attacks")]
    [SerializeField]
    private AttackAndCooldown _primaryAttack;
    
    [SerializeField]
    private AttackAndCooldown _secondaryAttack;
    
    public AttackAndCooldown Primary { get { return _primaryAttack; } }
    
    public AttackAndCooldown Secondary { get { return _secondaryAttack; } }
}

public class CrystalBall : MonoBehaviour
{
    [Header("Crystal Ball Attack Fail Particle")]
    [SerializeField]
    private ParticleSystem _failParticle;

    [Header("Crystal Ball Attacks")]
    [SerializeField]
    private List<AttackMode> _attackModes;
        
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _owner;
    
    [SerializeField]
    private AttackMode _current;
    private int _currentIndex;

    private Dictionary<Attack, float> _attackCooldowns = new Dictionary<Attack, float>(); // This isn't great, but it works

    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
        
        if(_owner == null)
            Debug.LogError($"No owning player found");
    }

    void Start()
    {
        _currentIndex = 0;
        _current = _attackModes[_currentIndex];
        
        foreach (AttackMode mode in _attackModes)
        {
            _attackCooldowns[mode.Primary.Attack] = 0;
            _attackCooldowns[mode.Secondary.Attack] = 0;
        }
    }
    
    void Update()
    {
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
            UseAttack(_current.Primary);

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
            UseAttack(_current.Secondary);

        if (Input.GetMouseButtonDown((int)MouseButton.Middle))
        {
            _currentIndex = (_currentIndex + 1) % _attackModes.Count;
            _current = _attackModes[_currentIndex];
        }
    }

    public void UseAttack(AttackAndCooldown attack)
    {
        if (!CanUseAttack(attack.Attack))
        {
            Instantiate(_failParticle, transform.position, Quaternion.identity);
            return;
        }

        AttackContext context = new AttackContext();
        context.Forward = _owner.Camera.transform.forward;
        attack.Attack.PerformAttack(_owner, context);
        _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    public bool CanUseAttack(Attack attack)
    {
        _attackCooldowns.TryGetValue(attack, out var cooldown);
        return cooldown <= 0;
    }
}
