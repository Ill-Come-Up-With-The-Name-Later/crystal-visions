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

    public event Action<bool> OnAimAssistToggle;
    
    private GameManager _gameManager;

    new void Awake()
    {
        base.Awake();
        
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

    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<PlayerAimAssist>() != null)
            {
                PlayerAimAssist aimAssist = gameObject.GetComponent<PlayerAimAssist>();
                aimAssist.Active = !aimAssist.Active;
                OnAimAssistToggle?.Invoke(aimAssist.Active);
            }
        }
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
