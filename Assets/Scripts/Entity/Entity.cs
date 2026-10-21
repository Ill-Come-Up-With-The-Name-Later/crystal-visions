using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    private EntityHealth _health;
    
    void Start()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
    }
}
