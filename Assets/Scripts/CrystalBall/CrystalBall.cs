using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum MouseButton
{
    Left,
    Right,
    Middle
}

public class CrystalBall : MonoBehaviour
{
    [Header("Crystal Ball Attacks")]
    [SerializeField]
    private AttackAndCooldown _closeRangeAttack;
    
    [SerializeField]
    private AttackAndCooldown _longRangeAttack;

    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _owner;

    private Dictionary<Attack, float> _attackCooldowns = new Dictionary<Attack, float>(); // This isn't great, but it works

    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
        
        if(_owner == null)
            Debug.LogError($"No owning player found");
    }

    void Start()
    {
        _attackCooldowns[_closeRangeAttack.Attack] = 0;
        _attackCooldowns[_longRangeAttack.Attack] = 0;
    }
    
    void Update()
    {
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
            UseAttack(_closeRangeAttack);

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
            UseAttack(_longRangeAttack);
    }

    public void UseAttack(AttackAndCooldown attack)
    {
        if (!CanUseAttack(attack.Attack))
            return;
        
        attack.Attack.PerformAttack(_owner);
        _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    public bool CanUseAttack(Attack attack)
    {
        _attackCooldowns.TryGetValue(attack, out var cooldown);
        return cooldown <= 0;
    }
}
