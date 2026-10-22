using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [Header("Parent Entity")]
    [SerializeField]
    private Entity _parent;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private BoxCollider _collider;

    public Entity Parent { get { return _parent; } }

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        
        if(_collider == null)
            Debug.LogWarning("No collider found");
        
        if(_parent == null) 
            Debug.LogWarning("No parent found");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hitbox"))
            return;
        
        Hitbox otherHitbox = other.gameObject.GetComponentInChildren<Hitbox>();
        
        if (otherHitbox != null)
        {
            Entity otherParent = otherHitbox.Parent;
            
            if (otherParent != _parent)
                Debug.Log(_parent.gameObject.name + " Hurtbox hit by " + otherParent.name);
        }
    }
}
