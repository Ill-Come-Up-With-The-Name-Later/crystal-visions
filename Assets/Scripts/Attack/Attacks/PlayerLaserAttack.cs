using System.Collections;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Player Laser")]
public class PlayerLaserAttack : Attack
{
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        if (source is Player player)
        {
            CrystalBall crystalBall = player.CrystalBall;
            
            if (crystalBall != null)
            {
                float range = source.AttackRangeStat.Value * 3;
                float damage = source.DamageStat.Value;
        
                RaycastHit[] hits;
                
                NativeArray<Vector3> points =  new NativeArray<Vector3>(2, Allocator.Temp);
                points[0] = crystalBall.gameObject.transform.position + crystalBall.gameObject.transform.forward * 0.5f;
                points[1] = crystalBall.gameObject.transform.position + (context == null ?
                    crystalBall.gameObject.transform.forward : context.Forward) * range;
                
                crystalBall.Laser.SetPositions(points);
                crystalBall.Laser.enabled = true;
        
                hits = Physics.RaycastAll( crystalBall.gameObject.transform.position + crystalBall.gameObject.transform.forward * 0.5f,
                    context == null ? crystalBall.gameObject.transform.forward : context.Forward, range);

                foreach (RaycastHit hit in hits)
                {
                    GameObject hitObject = hit.collider.gameObject;

                    if (hitObject.GetComponent<Hurtbox>() != null)
                    {
                        Entity hitEntity = hitObject.GetComponent<Hurtbox>().Parent;
                        hitEntity.Health.TakeDamage(damage * 4, source);
                    }
                }
                
                crystalBall.HideLaserCoroutine();
            }
        }
    }
}
