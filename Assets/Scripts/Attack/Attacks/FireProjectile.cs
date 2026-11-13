using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Projectile/Fire Projectile")]
public class FireProjectile : Attack
{
    [Header("Projectile")]
    [SerializeField]
    private Projectile _projectile;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        Vector3 forward = context?.Forward ?? source.transform.forward;
        
        Projectile.LaunchProjectile(_projectile,
            forward * _projectile.Speed, source);
    }
}
