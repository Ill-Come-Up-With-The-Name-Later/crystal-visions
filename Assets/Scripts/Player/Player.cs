using System;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private CrystalBall _crystalBall;
    
    public CrystalBall CrystalBall { get { return _crystalBall; } }
    
    [SerializeField]
    private PlayerCamera _camera;
    
    public PlayerCamera Camera { get { return _camera; } }
    
    [SerializeField]
    private ContestedHealth _contestedHealth;
    
    public ContestedHealth ContestedHealth { get { return _contestedHealth; } }

    public event Action OnPlayerDeath;
    
    private GameManager _gameManager;

    void Awake()
    {
        _health = GetComponent<EntityHealth>();
        
        if(_health == null)
            Debug.LogError("No health found");
        
        _rigidbody = GetComponent<Rigidbody>();
        
        if(_rigidbody == null)
            Debug.LogError("No rigidbody found");
        
        _contestedHealth = GetComponent<ContestedHealth>();
        
        if(_contestedHealth == null)
            Debug.LogError("No contested health found");
        
        _camera = GetComponentInChildren<PlayerCamera>();
        
        if(_camera == null)
            Debug.LogError("No camera found");
    }

    void Start()
    {
        _health.OnDeath += OnDeath;
        
        _gameManager = GameManager.Instance;
        
        _crystalBall = GetComponentInChildren<CrystalBall>();
        _gameManager.Crystal.Health.OnHealthChanges += GainDamageBoost;
    }

    private void GainDamageBoost()
    {
        float crystalHealth = GameManager.Instance.Crystal.Health.CurrentHealth;
        float crystalHealthPercent = crystalHealth / GameManager.Instance.Crystal.Health.MaxHealth;
        
        _damageStat.BonusValue = _damageStat.BaseValue * (1f - crystalHealthPercent);
    }

    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _gameManager.Crystal.Health.OnHealthChanges -= GainDamageBoost;
    }

    void OnDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}
