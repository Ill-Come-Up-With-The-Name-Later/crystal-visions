using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Basic Hitscan")]
public class BasicHitscanAttack : Attack
{
    [SerializeField]
    private ParticleSystem _meleeParticles;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        float range = source.AttackRangeStat.Value;
        float damage = source.DamageStat.Value;
        
        RaycastHit[] hits;
        
        hits = Physics.RaycastAll( source.transform.position + source.transform.forward * 0.5f,
            context == null ? source.transform.forward : context.Forward, range);
        
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
