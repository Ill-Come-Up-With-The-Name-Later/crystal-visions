using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Player Closer")]
public class PlayerCloser : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if (entity is Enemy enemy)
        {
            Vector3 enemyPos = enemy.transform.position;

            return Vector3.Distance(enemyPos, enemy.PlayerPosition) < 
                   Vector3.Distance(enemyPos, enemy.CrystalPosition);
        }
        
        return false;
    }
}
