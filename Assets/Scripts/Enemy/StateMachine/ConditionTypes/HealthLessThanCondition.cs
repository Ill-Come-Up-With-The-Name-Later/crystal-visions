using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Health Less Than Condition")]
public class HealthLessThanCondition : TransitionCondition
{
    [Header("Health")]
    [SerializeField]
    [Min(0)]
    private float _minHealth;
    
    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return entity.Health.CurrentHealth < _minHealth;
    }
}
