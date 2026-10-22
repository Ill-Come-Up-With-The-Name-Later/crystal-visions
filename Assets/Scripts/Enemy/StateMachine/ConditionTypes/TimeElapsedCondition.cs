using UnityEngine;

public class TimeElapsedCondition : TransitionCondition
{
    [Header("Time Elapsed Condition")]
    [SerializeField]
    private float _time;

    public override bool Evaluate(Entity entity, StateMachine stateMachine)
    {
        return stateMachine.TimeInState >= _time;
    }
}
