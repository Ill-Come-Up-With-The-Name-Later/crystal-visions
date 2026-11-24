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
        Vector3 forward = context?.Forward ?? source.transform.forward;
        Vector3 pos = context == null ? source.gameObject.transform.position + (forward * 1.5f) 
            : context.Position + forward;
        
        Instantiate(_meleeParticles, pos, source.transform.rotation);
        hits = Physics.RaycastAll( pos, forward, range);

        //Debug.Log($"Hitscan attack occurred. Forward vector: {forward} Position: {pos}");
        
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
