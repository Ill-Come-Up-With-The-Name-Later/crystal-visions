using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    private StateMachine _stateMachine;
    
    private Crystal _crystal;

    [SerializeField]
    private Vector3 _crystalPosition;
    
    public Vector3 CrystalPosition { get { return _crystalPosition; } }
    
    void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        
        if(_stateMachine == null)
            Debug.LogError("No state machine found");
        
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");
    }

    void Start()
    {
        _crystal = FindAnyObjectByType<Crystal>();
        
        if(_crystal == null)
            Debug.LogError("No Crystal found");
        
        _crystalPosition = new Vector3(_crystal.transform.position.x, 0, _crystal.transform.position.z);
    }
}
