using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Conditions/Time Elapsed Condition")]
public class TimeElapsedCondition : TransitionCondition
{
    [Header("Time Elapsed Condition")]
    [SerializeField]
    [Min(0)]
    private float _time;

    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return stateMachine != null && stateMachine.TimeInState >= _time;
    }
}
