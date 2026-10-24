using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Always False Condition")]
public class AlwaysFalse : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return false;
    }
}
