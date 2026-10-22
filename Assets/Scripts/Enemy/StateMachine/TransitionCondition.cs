using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    public abstract bool Evaluate(Entity entity, StateMachine stateMachine);
}
