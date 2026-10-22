using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("State Machine Settings")]
    [SerializeField]
    private State _startState;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _owner;

    [SerializeField]
    private State _currentState;
    
    [SerializeField]
    private State _previousState;

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
        if(_startState != null)
            _currentState = _startState;
    }
    
    void Update()
    {
        if (_currentState != null)
        {
            _timeInState += Time.deltaTime;
            
            foreach (var transition in _currentState.Transitions)
            {
                if (transition == null)
                    continue;

                if (transition.Condition.Evaluate(_owner, this))
                {
                    State next = transition.Next;
                    
                    if (next != null && next != _currentState)
                        SwapStates(next);
                }
            }
        }
    }

    private void SwapStates(State next)
    {
        _currentState.OnExit(this, next);
        _previousState = _currentState;
                        
        _currentState = next;
        _currentState.OnEnter(this, _previousState);
        _timeInState = 0;
    }
}
