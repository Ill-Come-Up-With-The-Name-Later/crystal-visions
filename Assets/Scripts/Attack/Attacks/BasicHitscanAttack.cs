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
        Vector3 forward = context == null ? source.transform.forward : context.Forward;
        
        hits = Physics.RaycastAll( source.transform.position + forward * 1.5f, 
            forward, range);
        
        Instantiate(_meleeParticles, source.transform.position + forward, 
            source.transform.rotation);

        //Debug.Log($"Hitscan attack occurred. Forward vector: {forward}");
        
        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<Hurtbox>() != null)
            {
                Entity hitEntity = hitObject.GetComponent<Hurtbox>().Parent;
                hitEntity.Health.TakeDamage(damage, source);
            }
        }
    }
}
