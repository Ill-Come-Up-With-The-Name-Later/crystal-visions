using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<Enemy>();
    }

    void Start()
    {
        _enemy.OnAttack += KillOnExplode;
    }

    void OnDestroy()
    {
        _enemy.OnAttack -= KillOnExplode;
    }

    void KillOnExplode()
    {
        _enemy.Health.TakeDamage(2_147_483_647, _enemy);
    }
}
