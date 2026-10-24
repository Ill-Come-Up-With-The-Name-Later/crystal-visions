using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Entity Stats")]
public class EntityStats : ScriptableObject
{
    [Min(1)]
    public float maxHealth;
    
    [Min(0)]
    public float speed;
    
    [Min(0)]
    public float hitInvulnerability;
    
    [Min(0)]
    public float attackRange; // For hitscan attacks
    
    [Min(0)]
    public float damage; // May not always apply
}
