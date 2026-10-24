using System;
using UnityEngine;

[Serializable]
public struct AttackAndCooldown
{
    [SerializeField]
    private Attack _attack;

    [SerializeField]
    private float _cooldown;
    
    public Attack Attack
    {
        get => _attack;
    }

    public float Cooldown
    {
        get => _cooldown;
    }
}

public abstract class Attack : ScriptableObject
{
    public abstract void PerformAttack(Entity source);
}
