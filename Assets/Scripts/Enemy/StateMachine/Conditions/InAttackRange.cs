using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/In Attack Range")]
public class InAttackRange : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if (entity is Enemy enemy)
        {
            Vector3 enemyPos = enemy.transform.position;
            EnemyStats stats = enemy.Stats as EnemyStats;

            if (stats == null)
                return false;

            double distance = Vector3.Distance(enemyPos, enemy.PlayerPosition);
            return distance < stats.MaxRange && distance > stats.MinRange;
        }

        return false;
    }
}
