using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _parent;
    
    [SerializeField]
    private BoxCollider _collider;
    
    public Entity Parent { get { return _parent; } set { _parent = value; } }

    public event Action<Entity, Entity> HitOccur;

    void Awake()
    {
        _parent = GetComponentInParent<Entity>();
        _collider = GetComponent<BoxCollider>();
        
        if(_collider == null)
            Debug.LogError("No collider found");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hurtbox"))
            return;
        
        Hurtbox otherHurtbox = other.gameObject.GetComponentInChildren<Hurtbox>();

        if (otherHurtbox == null)
            return;

        if (otherHurtbox.Parent == null)
            return;
        
        if (otherHurtbox.Parent.Health.Invulnerable)
            return;

        if (otherHurtbox.Parent == _parent)
            return;
        
        HitOccur?.Invoke(otherHurtbox.Parent, _parent);
    }
    
    void Start()
    {
    }
    
    void Update()
    {
    }
}
