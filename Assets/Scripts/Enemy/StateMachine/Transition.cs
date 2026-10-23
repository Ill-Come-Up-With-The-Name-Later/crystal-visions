using System;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition")]
public class Transition : ScriptableObject
{
    [Header("Transition Condition")]
    [SerializeField]
    protected TransitionCondition _condition;
    
    public TransitionCondition Condition { get { return _condition; } }
    
    [SerializeField]
    protected StateNode _next;
    
    public StateNode Next { get { return _next; } }
}
