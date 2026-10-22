using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/State Node")]
public class StateNode : ScriptableObject
{
    [SerializeField]
    private State _state;
    
    public State State { get => _state; }
    
    [Header("Transitions")]
    [SerializeField]
    private List<Transition> _transitions;
    
    public IReadOnlyList<Transition> Transitions { get => _transitions; }
}

public abstract class State : ScriptableObject
{
    public abstract void OnEnter(StateMachine stateMachine, State previousState);

    public abstract void Tick(StateMachine stateMachine, float deltaTime);

    public abstract void FixedTick(StateMachine stateMachine, float deltaTime);

    public abstract void OnExit(StateMachine stateMachine, State nextState);
}
