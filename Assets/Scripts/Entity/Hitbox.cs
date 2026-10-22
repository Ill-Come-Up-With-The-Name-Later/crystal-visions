using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _parent;
    
    [SerializeField]
    private BoxCollider _collider;
    
    public Entity Parent { get { return _parent; } }
    
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        
        if(_collider == null)
            Debug.LogWarning("No collider found");
    }
    
    void Update()
    {
        
    }
}
