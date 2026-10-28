using System;
using UnityEngine;

public class ContestedHealth : MonoBehaviour
{
    [Header("Grace Period")] 
    [SerializeField]
    private float _contestedHealthGracePeriod;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private float _contestedHealthAmount;
    
    public float ContestedHealthAmount { get { return _contestedHealthAmount; } }

    [SerializeField]
    private float _drainRate;
    
    [SerializeField]
    private Entity _owner;
    
    [SerializeField]
    private EntityHealth _entityHealth;

    public event Action<float> OnContestedHealthChange;

    void Awake()
    {
        _owner = gameObject.GetComponent<Entity>();
        
        if(_owner == null)
            Debug.Log("No entity parent found");

        _entityHealth = _owner.GetComponent<EntityHealth>();
        
        if(_entityHealth == null)
            Debug.Log("No entity health found");
        
        _entityHealth.OnDamageTaken += GrantContestedHealth;
        EntityHealth.DamageTakenAdditionalInfo += ReturnContestedHealth;
    }

    void Update()
    {
        if (_owner.TimeSinceLastDamage >= _contestedHealthGracePeriod)
        {
            _contestedHealthAmount = Math.Max(_contestedHealthAmount - (Time.deltaTime * _drainRate), 0);
            OnContestedHealthChange?.Invoke(_contestedHealthAmount);
        }

        if (_contestedHealthAmount < 0)
            _contestedHealthAmount = 0;
    }

    void GrantContestedHealth(float amount)
    {
        _contestedHealthAmount = Math.Min(_contestedHealthAmount + amount, _owner.Health.MaxHealth - 1);
        OnContestedHealthChange?.Invoke(_contestedHealthAmount);
    }

    void ReturnContestedHealth(float damage, Entity damaged, Entity source)
    {
        if (source != _owner)
            return;

        float returnAmount;
        
        if (damage > _contestedHealthAmount)
            returnAmount = _contestedHealthAmount;
        else
            returnAmount = _contestedHealthAmount - damage;
        
        _contestedHealthAmount = Math.Max(_contestedHealthAmount - damage, 0);
        
        OnContestedHealthChange?.Invoke(_contestedHealthAmount);
        source.Health.Heal(returnAmount);
    }
}
