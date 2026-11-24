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

    [Header("Player Exclusive Stats")]
    [SerializeField]
    private EntityStat _healthRegeneration;

    [SerializeField]
    private EntityStat _jumpStrength;

    [SerializeField]
    private bool _jumpBuffer;

    public event Action OnPlayerDeath;

    public event Action<bool> OnAimAssistToggle;
    
    private GameManager _gameManager;

    protected override void Awake()
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
        
        if(_crystalBall == null)
            _crystalBall = GetComponentInChildren<CrystalBall>();
        
        _gameManager.Crystal.Health.OnHealthChanges += GainStatBoosts;
        _health.OnHealthChanges += GainStatBoosts;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gameObject.GetComponent<PlayerAimAssist>() != null)
            {
                PlayerAimAssist aimAssist = gameObject.GetComponent<PlayerAimAssist>();
                aimAssist.Active = !aimAssist.Active;
                OnAimAssistToggle?.Invoke(aimAssist.Active);
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space) && OnGround()) || (_jumpBuffer && OnGround()))
            Jump();
        else if (Input.GetKeyDown(KeyCode.Space) && 
                 (transform.position.y < 0.7f && _rigidbody.linearVelocity.y < 0))
            _jumpBuffer = true;
        
        _health.Heal(_healthRegeneration.Value * Time.deltaTime, EntityHealth.HealReason.Regeneration);
    }

    bool OnGround()
    {
        return transform.position.y + 0.05 >= 0.55f && transform.position.y - 0.05 <= 0.55f;
    }

    private void Jump()
    {
        _rigidbody.linearVelocity += new Vector3(0, _jumpStrength.Value, 0);
        _jumpBuffer = false;
    }

    private void GainStatBoosts()
    {
        float crystalHealth = GameManager.Instance.Crystal.Health.CurrentHealth;
        float crystalHealthPercent = crystalHealth / GameManager.Instance.Crystal.Health.MaxHealth;
        float currentHealthPercent = _health.CurrentHealth / _health.MaxHealth;
        
        _damageStat.BonusValue = _damageStat.BaseValue * (1f - crystalHealthPercent) * 2f;
        _speedStat.BonusValue = (_speedStat.BaseValue * (1f - crystalHealthPercent)) * 0.4f;
        _healthRegeneration.BonusValue = _healthRegeneration.BaseValue * (1f - crystalHealthPercent);
        _damageStat.Multiplier = (1f - currentHealthPercent) * 0.8f;
    }

    protected override void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHealthChanges -= GainStatBoosts;
        _gameManager.Crystal.Health.OnHealthChanges -= GainStatBoosts;
    }

    void OnDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}
