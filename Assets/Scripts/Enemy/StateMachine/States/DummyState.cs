using UnityEngine;

[CreateAssetMenu(menuName="StateMachine/States/Dummy State")]
public class DummyState : State
{
    public override void OnEnter(StateMachine stateMachine, State nextState)
    {
    }

    public override void OnExit(StateMachine stateMachine, State nextState)
    {
    }

    public override void FixedTick(StateMachine stateMachine, float deltaTime)
    {
    }

    public override void Tick(StateMachine stateMachine, float deltaTime)
    {
    }
}
