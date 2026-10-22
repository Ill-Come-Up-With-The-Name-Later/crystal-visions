using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Entity Stats")]
public class EntityStats : ScriptableObject
{
    public float maxHealth;
    public float speed;
    public float hitInvulnerability;
    public float damage;
}
