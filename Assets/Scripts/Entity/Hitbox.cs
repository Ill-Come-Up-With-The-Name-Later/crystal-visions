using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _parent;
    
    [SerializeField]
    private Collider _collider;
    
    public Entity Parent { get { return _parent; } set { _parent = value; } }

    public event Action<Entity, Entity> HitOccur;

    void Awake()
    {
        _parent = GetComponentInParent<Entity>();
        _collider = GetComponent<Collider>();
        
        if(_collider == null)
            Debug.LogError("No collider found");
    }
    
    void Update()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.x = 0f; 
        transform.localEulerAngles = currentRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hurtbox"))
            return;
        
        Hurtbox otherHurtbox = other.gameObject.GetComponentInChildren<Hurtbox>();
        
        if (otherHurtbox == null)
            return;
        
        Entity target = otherHurtbox.Parent;
        
        if (_parent == null)
            return;

        if (target == null || target == _parent)
            return;
        
        HitOccur?.Invoke(target, _parent);
    }
}