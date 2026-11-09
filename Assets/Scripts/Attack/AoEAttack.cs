using UnityEngine;

public class AoEAttack : Attack
{
    [Header("Attack Parameters")]
    [SerializeField]
    private float _radius;
    
    [SerializeField]
    private ParticleSystem _particles;

    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Vector3 _position;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        Instantiate(_particles, _position, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(_position, _radius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<Hurtbox>() != null)
            {
                Hurtbox hurtbox = collider.gameObject.GetComponent<Hurtbox>();

                if (hurtbox == null)
                    continue;

                if (hurtbox.Parent != source)
                    hurtbox.Parent.Health.TakeDamage(source.DamageStat.Value, source);
            }
        }
    }
}
