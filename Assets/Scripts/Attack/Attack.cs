using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public float cooldown;
    
    public abstract void PerformAttack(Entity source);
}
