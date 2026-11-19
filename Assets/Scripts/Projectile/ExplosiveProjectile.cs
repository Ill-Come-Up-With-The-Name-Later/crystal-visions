using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [Header("Explosion Effect")]
    [SerializeField]
    private AoEAttack explosion;

    protected override void OnProjectileHit(Entity hit, Entity source)
    {
        base.OnProjectileHit(hit, source);
        
        if (hit == source)
            return;

        AttackContext context = new AttackContext();
        context.Position = gameObject.transform.position;
        
        if(explosion != null)
            explosion.PerformAttack(source, context);
        
        //Debug.Log($"Explosive projectile hit {hit}");
    }
}
