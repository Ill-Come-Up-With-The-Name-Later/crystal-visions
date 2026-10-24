using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/On Attack Cooldown")]
public class OnAttackCooldown : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if (entity is Enemy enemy)
            return !enemy.CanAttack();

        return false;
    }
}