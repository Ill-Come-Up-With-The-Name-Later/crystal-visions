using System.Collections;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] 
    private float _maxHealth;
    
    [SerializeField] 
    private float _currentHealth;
    
    private bool _invulnerable;
    
    void Start()
    {
        _currentHealth = _maxHealth;
    }
    
    void Update()
    {
        
    }

    public IEnumerator MakeInvulnerable(float duration)
    {
        _invulnerable = true;
        yield return new WaitForSeconds(duration);
        _invulnerable = false;
    }

    public void TakeDamage(float damage, IDamageable source)
    {
        if(_invulnerable) 
            return;
        
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(float heal)
    {
        _currentHealth += heal;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
