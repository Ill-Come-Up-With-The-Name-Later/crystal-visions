using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Values")]
    [SerializeField] 
    private float _maxHealth;
    
    [SerializeField] 
    private float _currentHealth;
    
    [Header("Invulnerability")]
    [SerializeField]
    private bool _invulnerable;

    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Slider _healthbar;
    
    void Start()
    {
        _currentHealth = _maxHealth;
        _healthbar = gameObject.GetComponentInChildren<Slider>();
        
        if(_healthbar == null)
            Debug.LogError("Health bar not found on " + gameObject.name);
        
        _healthbar.value = 1f;
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

    public void TakeDamage(float damage, Entity source)
    {
        if(_invulnerable) 
            return;
        
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
            Die();

        _healthbar.value = _currentHealth / _maxHealth;
    }

    public void Heal(float heal)
    {
        _currentHealth += heal;
        _healthbar.value = _currentHealth / _maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
