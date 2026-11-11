using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Player Lifesteal Beam")]
public class PlayerLifestealLaser : Attack
{
    [Header("Attack Settings")]
    [SerializeField]
    private HealBeam _healBeam;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        if (source is Player player)
        {
            CrystalBall crystalBall = player.CrystalBall;
            
            if (crystalBall != null)
            {
                float range = source.AttackRangeStat.Value * 2;
                float damage = source.DamageStat.Value / 2f;
        
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
                        
                        HealBeam beam = Instantiate(_healBeam, source.transform.position, Quaternion.identity)
                            .GetComponent<HealBeam>();
                        beam.Source = hitEntity;
                        beam.Target = source;
                        beam.HealAmount = damage / 2;
                        
                        beam.Heal();
                        
                        HealBeam beam2 = Instantiate(_healBeam, source.transform.position, Quaternion.identity)
                            .GetComponent<HealBeam>();
                        beam2.Source = hitEntity;
                        beam2.Target = GameManager.Instance.Crystal;
                        beam2.HealAmount = damage / 2;
                        
                        beam2.Heal();
                    }
                }
                
                crystalBall.HideLaserCoroutine();
            }
        }
    }
}