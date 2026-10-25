using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("State Machine Settings")]
    [SerializeField]
    private StateNode _startState;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _owner;
    
    [SerializeField]
    private StateNode _currentState;

    [SerializeField]
    private StateNode _previousState;

    [SerializeField]
    private float _timeInState;
    
    public float TimeInState { get { return _timeInState; } }
    
    public Entity Owner { get { return _owner; } }
    
    void Awake()
    {
        _owner = gameObject.GetComponent<Entity>();
    }
    
    void Start()
    {
        if (_startState == null || _startState.State == null)
            return;
        
        _currentState = _startState;
        _currentState.State.OnEnter(this, null);
    }
    
    void Update()
    {
        if (_currentState?.State == null)
            return;
        
        _timeInState += Time.deltaTime;
        _currentState.State.Tick(this, Time.deltaTime);
        
        foreach (var transition in _currentState.Transitions)
        {
            if (transition == null)
                continue;
            
            if(transition.Condition == null)
                continue;
            
            if(transition.Next == null || transition.Next.State == null)
                continue;

            if (transition.Condition.Evaluate(_owner, this))
            {
                StateNode next = transition.Next;
                SwapStates(next);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentState?.State == null)
            return;
        
        _currentState.State.FixedTick(this, Time.fixedDeltaTime);
    }

    private void SwapStates(StateNode next)
    {
        if (next == null || next == _currentState || next.State == null)
            return;
        
        _currentState.State.OnExit(this, next.State);
        _previousState = _currentState;

        Debug.Log($"Swapped to {next.State.name} from {_currentState.State.name}");
        
        _currentState = next;
        _currentState.State.OnEnter(this, _previousState.State);
        _timeInState = 0;
    }
}
