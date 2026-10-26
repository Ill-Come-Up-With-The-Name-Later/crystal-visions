using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Basic Hitscan")]
public class BasicHitscanAttack : Attack
{
    [SerializeField]
    private ParticleSystem _meleeParticles;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        float range = source.Stats.attackRange;
        float damage = source.Damage;

        RaycastHit[] hits;

        if (context == null)
        {
            hits = Physics.RaycastAll(source.transform.position,
                source.transform.forward, range);
        }
        else
        {
            hits = Physics.RaycastAll(source.transform.position,
                context.Forward, range);
        }
        
        Instantiate(_meleeParticles, source.transform.position + source.transform.forward, source.transform.rotation);

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<Entity>() != null)
            {
                Entity hitEntity = hitObject.GetComponent<Entity>();
                hitEntity.Health.TakeDamage(damage, source);
            }
        }
    }
}
