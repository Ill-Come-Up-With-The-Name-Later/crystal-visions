using System;
using UnityEngine;

[Serializable]
public abstract class Transition : MonoBehaviour
{
    [Header("Transition Condition")]
    [SerializeField]
    protected TransitionCondition _condition;
    
    public TransitionCondition Condition { get { return _condition; } }
    
    [SerializeField]
    protected State _next;
    
    public State Next { get { return _next; } }
}
