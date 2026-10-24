using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Basic Hitscan")]
public class BasicHitscanAttack : Attack
{
    public override void PerformAttack(Entity source)
    {
        float range = source.Stats.attackRange;
        float damage = source.Stats.damage;

        RaycastHit[] hits = Physics.RaycastAll(source.transform.position,
            source.transform.forward, range);

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
