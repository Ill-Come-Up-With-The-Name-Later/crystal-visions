using UnityEngine;

public class OrCondition : TransitionCondition
{
    [Header("Conditions")]
    [SerializeField]
    private TransitionCondition _conditionOne;
    
    [SerializeField]
    private TransitionCondition _conditionTwo;

    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return _conditionOne.Evaluate(entity, stateMachine) || _conditionTwo.Evaluate(entity, stateMachine);
    }
}
