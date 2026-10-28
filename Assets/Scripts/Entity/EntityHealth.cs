using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] 
    private float _currentHealth;

    [SerializeField]
    private float _healthBonus;

    [SerializeField]
    private float multiplicativeHealthBonus;
    
    [Header("Invulnerability")]
    [SerializeField]
    private bool _invulnerable;

    [Header("Debug (Do not edit)")]
    [SerializeField] 
    private float _maxHealth;
    
    [SerializeField]
    private Slider _healthbar;
    
    [SerializeField]
    private Entity _entity;
    
    public bool Invulnerable { get { return _invulnerable; } }
    
    public float CurrentHealth { get { return _currentHealth; } }
    
    public float MaxHealth { get { return _maxHealth; } }

    public event Action<float> OnDamageTaken;

    public static event Action<float, Entity, Entity> DamageTakenAdditionalInfo;

    public event Action<float> OnHeal;

    public event Action OnDeath;

    public static event Action<Entity> EntityDies;

    public event Action OnHealthChanges;
    
    void Start()
    {
        _entity = GetComponent<Entity>();

        if (_entity is Enemy)
        {
            _healthBonus = WaveManager.Instance.CurrentWave * 5;
            multiplicativeHealthBonus = 0.15f * WaveManager.Instance.CurrentWave;
        }

        _maxHealth = (_entity.Stats.maxHealth + _healthBonus) * (1 + multiplicativeHealthBonus);
        _currentHealth = _maxHealth;
        _healthbar = gameObject.GetComponentInChildren<Slider>();
        
        if(_healthbar == null)
            Debug.LogError("Health bar not found on " + gameObject.name);
        
        _healthbar.value = 1f;
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

        if (_entity is Crystal && source is Player)
            return;
        
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
            Die();

        _healthbar.value = _currentHealth / _maxHealth;
        _entity.TimeSinceLastDamage = 0;
        StartCoroutine(MakeInvulnerable(_entity.Stats.hitInvulnerability));
        OnDamageTaken?.Invoke(damage);
        OnHealthChanges?.Invoke();
        DamageTakenAdditionalInfo?.Invoke(damage, _entity, source);
    }

    public void Heal(float heal)
    {
        Debug.Log($"Attempting to heal {_entity.name} by {heal} health.");
        
        _currentHealth = Math.Min(_currentHealth + heal, _maxHealth); // How did I forget to cap this???
        _healthbar.value = _currentHealth / _maxHealth;
        OnHeal?.Invoke(heal);
        OnHealthChanges?.Invoke();
    }

    public void Die()
    {
        OnDeath?.Invoke();
        EntityDies?.Invoke(_entity);
    }
}
