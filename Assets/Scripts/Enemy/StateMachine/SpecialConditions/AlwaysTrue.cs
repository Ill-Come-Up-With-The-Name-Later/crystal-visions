using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Composite/Always True Condition")]
public class AlwaysTrue : TransitionCondition
{
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return true;
    }
}
