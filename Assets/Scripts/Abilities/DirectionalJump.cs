using UnityEngine;

public class DirectionalJump : PeriodicAction
{
    [Header("Directional Jump Parameters")]
    [SerializeField]
    private float _jumpStrength;

    [SerializeField]
    private float _jumpSpeed;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _entity;
    
    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    protected override void PerformAction()
    {
        _entity.Rigidbody.linearVelocity += (_entity.transform.forward + new Vector3(0, _jumpStrength, 0)) * _jumpSpeed;
    }
}
