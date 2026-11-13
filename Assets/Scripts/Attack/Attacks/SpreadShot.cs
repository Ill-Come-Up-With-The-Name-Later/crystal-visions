using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Projectile/Spread Shot Attack")]
public class SpreadShot : Attack
{
    [Header("Projectile")]
    [SerializeField]
    private Projectile _projectile;

    [Min(2)]
    [SerializeField]
    private int _projectiles;

    [Header("Projectile Spread (Degrees)")]
    [SerializeField]
    private float _spread;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        Vector3 forward = context?.Forward ?? source.transform.forward;

        float totalAngle = _spread * (_projectiles - 1);
        float halfAngle = totalAngle / 2f;
        float increment = totalAngle / (_projectiles - 1);

        for (int i = 0; i < _projectiles; i++)
        {
            float angle = -halfAngle + increment * i;

            Projectile.LaunchProjectile(_projectile, 
                Quaternion.AngleAxis(angle, Vector3.up) * forward * _projectile.Speed, source);
        }
    }
}