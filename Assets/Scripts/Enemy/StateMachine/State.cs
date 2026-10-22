using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class State : MonoBehaviour
{
    [Header("Transitions")]
    [SerializeField]
    protected List<Transition> _transitions;
    
    [Header("Debug (Do not edit")]
    [SerializeField]
    protected Entity _owner;
    
    public Entity Owner { get { return _owner; } }
    
    public List<Transition> Transitions { get { return _transitions; } }
    
    public virtual void OnEnter(StateMachine stateMachine, State previousState)
    {
        _owner = stateMachine.Owner;
        Debug.Log("Entered " + name + " from " + previousState.name);
    }

    public abstract void Tick(float deltaTime);

    public abstract void FixedTick(float deltaTime);

    public abstract void OnExit(StateMachine stateMachine, State nextState);
}
