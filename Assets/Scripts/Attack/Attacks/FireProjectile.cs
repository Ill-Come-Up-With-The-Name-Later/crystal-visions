using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Projectile/Fire Projectile")]
public class FireProjectile : Attack
{
    [Header("Projectile")]
    [SerializeField]
    private Projectile _projectile;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        if (context == null)
        {
            Projectile.LaunchProjectile(_projectile,
                source.transform.forward * _projectile.Speed, source);
        }
        else
        {
            Projectile.LaunchProjectile(_projectile,
                context.Forward * _projectile.Speed, source);
        }
    }
}
