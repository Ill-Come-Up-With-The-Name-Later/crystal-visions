using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Crystal Closer")]
public class CrystalCloser : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if (entity is Enemy enemy)
        {
            Vector3 enemyPos = enemy.transform.position;

            return Vector3.Distance(enemyPos, enemy.CrystalPosition) < 
                   Vector3.Distance(enemyPos, enemy.PlayerPosition);
        }
        
        return false;
    }
}
