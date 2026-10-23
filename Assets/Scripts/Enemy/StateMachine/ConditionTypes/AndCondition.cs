using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Composite/And Condition")]
public class AndCondition : TransitionCondition
{
    [Header("Conditions")]
    [SerializeField]
    private TransitionCondition _conditionOne;
    
    [SerializeField]
    private TransitionCondition _conditionTwo;

    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        if(_conditionOne == null || _conditionTwo == null)
            return false;
        
        return _conditionOne.Evaluate(entity, stateMachine) && _conditionTwo.Evaluate(entity, stateMachine);
    }
}
