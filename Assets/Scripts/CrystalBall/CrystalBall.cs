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
    private Attack _closeRangeAttack;
    
    [SerializeField]
    private Attack _longRangeAttack;

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
        _attackCooldowns[_closeRangeAttack] = 0;
        _attackCooldowns[_longRangeAttack] = 0;
    }
    
    void Update()
    {
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
            Attack(_closeRangeAttack);

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
            Attack(_longRangeAttack);
    }

    public void Attack(Attack attack)
    {
        if (!CanUseAttack(attack))
            return;
        
        attack.PerformAttack(_owner);
        _attackCooldowns[attack] = attack.cooldown;
    }

    public bool CanUseAttack(Attack attack)
    {
        _attackCooldowns.TryGetValue(attack, out var cooldown);
        return cooldown <= 0;
    }
}
