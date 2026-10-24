using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Composite/Negate Condition")]
public class NegateCondition : TransitionCondition
{
    [Header("Condition")]
    [SerializeField]
    private TransitionCondition _condition;

    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if (_condition == null)
            return false;
        
        return !_condition.Evaluate(entity, stateMachine);
    }
}
