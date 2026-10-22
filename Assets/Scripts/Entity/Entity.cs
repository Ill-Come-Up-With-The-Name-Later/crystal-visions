using UnityEngine;

public class Entity : MonoBehaviour
{
    private EntityHealth _health;
    
    public EntityHealth Health { get { return _health; } }
    
    void Start()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
    }
}
