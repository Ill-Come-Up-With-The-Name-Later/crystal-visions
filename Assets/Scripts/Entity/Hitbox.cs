using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Entity _parent;
    
    [SerializeField]
    private BoxCollider _collider;
    
    public Entity Parent { get { return _parent; } }

    void Awake()
    {
        _parent = GetComponentInParent<Entity>();
        _collider = GetComponent<BoxCollider>();
        
        if(_parent == null)
            Debug.LogError("No parent found");
        
        if(_collider == null)
            Debug.LogError("No collider found");
    }
    
    void Start()
    {
    }
    
    void Update()
    {
    }
}
