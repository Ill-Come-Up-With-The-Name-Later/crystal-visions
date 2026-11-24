using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Hitscan/Spread Hitscan")]
public class SpreadMelee : Attack
{
    [Header("Attack")]
    [SerializeField]
    private BasicHitscanAttack _attacak;

    [Min(2)]
    [SerializeField]
    private int _attacks;

    [Header("Projectile Spread (Degrees)")]
    [SerializeField]
    private float _spread;
    
    public override void PerformAttack(Entity source, AttackContext context = null)
    {
        Vector3 forward = context?.Forward ?? source.transform.forward;
        Vector3 position = context?.Position ?? source.transform.position;

        float totalAngle = _spread * (_attacks - 1);
        float halfAngle = totalAngle / 2f;
        float increment = totalAngle / (_attacks - 1);

        for (int i = 0; i < _attacks; i++)
        {
            float angle = -halfAngle + increment * i;
            Vector3 finalForward = (Quaternion.AngleAxis(angle, Vector3.up) * forward).normalized;

            AttackContext attackContext = new AttackContext(finalForward, position);
            
            _attacak.PerformAttack(source, attackContext);
        }
    }
}