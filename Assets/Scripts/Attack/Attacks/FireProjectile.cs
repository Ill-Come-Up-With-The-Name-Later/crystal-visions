using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Fire Projectile")]
public class FireProjectile : Attack
{
    [Header("Projectile")]
    [SerializeField]
    private Projectile _projectile;
    
    public override void PerformAttack(Entity source)
    {
        Projectile.LaunchProjectile(_projectile, 
            source.transform.forward * _projectile.Speed, source);
    }
}
