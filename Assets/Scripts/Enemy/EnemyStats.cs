using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats")]
public class EnemyStats : EntityStats
{
    [Min(0)]
    public float MinRange;
    
    [Min(0)]
    public float MaxRange;
}
