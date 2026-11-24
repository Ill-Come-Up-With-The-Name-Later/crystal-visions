using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    public enum HealReason
    {
        Regeneration,
        Ability
    }
    
    [Header("Health")]
    [SerializeField] 
    private float _currentHealth;
    
    [Header("Damage Particle")]
    [SerializeField]
    private ParticleSystem _damageParticle;

    [SerializeField]
    private ParticleSystem _deathParticle;

    [Header("Healing Particle")]
    [SerializeField]
    private ParticleSystem _healParticle;
    
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

    [SerializeField]
    private bool _dead;
    
    public bool Invulnerable { get { return _invulnerable; } }
    
    public float CurrentHealth { get { return _currentHealth; } }
    
    public float MaxHealth { get { return _maxHealth; } }
    
    public float HealthPercentage {  get { return _currentHealth / _maxHealth; } }

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
            _entity.HealthStat.BonusValue = WaveManager.Instance.CurrentWave * 5;
            _entity.HealthStat.Multiplier = 0.2f * WaveManager.Instance.CurrentWave;
        }

        _maxHealth = _entity.HealthStat.Value;
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

    public void TakeDamage(float damage, Entity source, bool overrideImmunity = false)
    {
        if(_invulnerable && !overrideImmunity) 
            return;

        if (_dead)
            return;

        if (_entity == null)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        if (_entity is Crystal && source is Player)
            return;
        
        _currentHealth = Math.Max(_currentHealth - damage, 0);
        
        if(_damageParticle != null)
            Instantiate(_damageParticle, _entity.transform.position, Quaternion.identity);

        if (_currentHealth <= 0)
        {
            if (_entity.GetComponentInChildren<ContestedHealth>() != null)
            {
                ContestedHealth health = _entity.GetComponentInChildren<ContestedHealth>();
                health.DamageContestedHealth(damage * 1.5f, source);
                
                if(health.ContestedHealthAmount <= 0)
                    Die();

                return;
            }
            
            Die();
        }

        _healthbar.value = _currentHealth / _maxHealth;
        _entity.TimeSinceLastDamage = 0;
        
        StartCoroutine(MakeInvulnerable(_entity.HitInvulnerabilityStat.Value));
        
        OnDamageTaken?.Invoke(damage);
        OnHealthChanges?.Invoke();
        DamageTakenAdditionalInfo?.Invoke(damage, _entity, source);
    }

    public void Heal(float heal, HealReason reason)
    {
        //Debug.Log($"Attempting to heal {_entity.name} by {heal} health.");
        
        _currentHealth = Math.Min(_currentHealth + heal, _maxHealth); // How did I forget to cap this???
        _healthbar.value = _currentHealth / _maxHealth;
        OnHeal?.Invoke(heal);
        OnHealthChanges?.Invoke();
        
        if(_healParticle != null && reason != HealReason.Regeneration)
            Instantiate(_healParticle, _entity.transform.position, Quaternion.identity);
    }

    public void Die()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (_entity == null)
            return;

        if (!gameObject.activeSelf)
            return;
        
        if(_deathParticle != null)
            Instantiate(_deathParticle, _entity.transform.position, Quaternion.identity);

        _dead = true;
        OnDeath?.Invoke();
        EntityDies?.Invoke(_entity);
    }
}
