using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hurtbox : MonoBehaviour
{
    [Header("Parent Entity")]
    [SerializeField]
    private Entity _parent;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Collider _collider;

    public Entity Parent { get { return _parent; } }

    void Awake()
    {
        _collider = GetComponent<Collider>();
        _parent = GetComponentInParent<Entity>();
        
        if(_collider == null)
            Debug.LogWarning("No collider found");
        
        if(_parent == null) 
            Debug.LogWarning("No parent found");
    }

    void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    void OnTriggerStay(Collider other)
    {
       TryDamage(other);
    }

    private void TryDamage(Collider other)
    {
        if (!other.CompareTag("Hitbox"))
            return;
        
        if (_parent == null)
            return;
        
        if(_parent.Health == null)
            return;
        
        if (_parent.Health.Invulnerable)
            return;
        
        Hitbox otherHitbox = other.gameObject.GetComponentInChildren<Hitbox>();
        
        if (otherHitbox == null || otherHitbox.Parent == null)
            return;
        
        Entity otherParent = otherHitbox.Parent;
        
        if (otherParent == null || otherParent == _parent)
            return;

        if (otherParent.Stats == null)
            return;
        
        _parent.Health.TakeDamage(otherParent.Stats.damage, otherParent);
        //Debug.Log(_parent.gameObject.name + " Hurtbox hit by " + otherParent.name);
    }
}
