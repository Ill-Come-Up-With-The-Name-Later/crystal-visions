using System;
using UnityEngine;

public class CaligneaBoss : MonoBehaviour
{
    [Header("Calignea Specials")]
    [SerializeField]
    private float _maxPatience;
    
    public float MaxPatience { get => _maxPatience; }
    
    [SerializeField]
    private CaligneaBoundary _boundary;

    [SerializeField]
    private float _boundaryScalingRate;

    [SerializeField]
    private float _patienceDrainRate;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private float _currentPatience;
    
    public float CurrentPatience { get => _currentPatience; set => _currentPatience = value; }
    
    [SerializeField] 
    private Enemy _enemy;
    public Enemy Enemy { get { return _enemy; } }
    
    public float CurrentHealth { get => _enemy.Health.CurrentHealth; }
    public float MaxHealth { get => _enemy.Health.MaxHealth; }
    
    private EntityHealth Health { get => _enemy.Health; }

    public event Action<float> OnPatienceChange;
    public static event Action OnPatienceExpires;
    public event Action OnDeath;
    public static event Action<CaligneaBoss> OnBossSpawned;

    public event Action OnHealthChanged;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<Enemy>();
        _boundary = gameObject.GetComponentInChildren<CaligneaBoundary>();
    }

    void Start()
    {
        _currentPatience = _maxPatience;
        
        OnPatienceChange += GainStatBoosts;
        OnBossSpawned?.Invoke(this);
        Health.OnHealthChanges += TriggerHealthChangeEvent;
        _enemy.Health.OnDamageTaken += ShrinkBoundary;
    }
    
    void Update()
    {
        _currentPatience = Mathf.Max(_currentPatience - (_patienceDrainRate * Time.deltaTime), 0);
        OnPatienceChange?.Invoke(_currentPatience);
        
        Vector3 scale = _boundary.transform.localScale;
        
        scale.x += _boundaryScalingRate * Time.deltaTime;
        scale.y += _boundaryScalingRate * Time.deltaTime;
        scale.z += _boundaryScalingRate * Time.deltaTime;
        
        _boundary.transform.localScale = scale;

        if (_currentPatience <= 0)
        {
            OnPatienceExpires?.Invoke();
            GameManager.Instance.Player.Health.TakeDamage(2_147_483_647, _enemy);
        }
    }

    void ShrinkBoundary(float damage)
    {
        Vector3 scale = _boundary.transform.localScale;

        scale.x = Math.Max(3, scale.x - (damage * 0.002f));
        scale.y = Math.Max(2, scale.y - (damage * 0.002f));
        scale.z = Math.Max(3, scale.z - (damage * 0.002f));
        
        _boundary.transform.localScale = scale;
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        OnPatienceChange -= GainStatBoosts;
        Health.OnHealthChanges -= TriggerHealthChangeEvent;
        _enemy.Health.OnDamageTaken -= ShrinkBoundary;
    }

    void TriggerHealthChangeEvent()
    {
        OnHealthChanged?.Invoke();
    }

    private void GainStatBoosts(float currentPatience)
    {
        float patiencePercent = _currentPatience / _maxPatience;
        
        _enemy.SpeedStat.BonusValue = (1f - patiencePercent) * 5;
        _enemy.DamageStat.BonusValue = (1f - patiencePercent) * _enemy.DamageStat.BaseValue * 0.15f;
    }
}
