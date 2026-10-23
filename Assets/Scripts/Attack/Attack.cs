using UnityEngine;

public enum AttackType
{
    Hitscan,
    Projectile,
    AreaOfEffect,
}

public abstract class Attack : ScriptableObject
{
    [Header("Attack Type")]
    [SerializeField]
    protected AttackType _type;
    
    public abstract void PerformAttack(Entity source);
}
