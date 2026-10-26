using UnityEngine;

public abstract class State : ScriptableObject
{
    public abstract void OnEnter(StateMachine stateMachine, State previousState);

    public abstract void Tick(StateMachine stateMachine, float deltaTime);

    public abstract void FixedTick(StateMachine stateMachine, float deltaTime);

    public abstract void OnExit(StateMachine stateMachine, State nextState);
}
