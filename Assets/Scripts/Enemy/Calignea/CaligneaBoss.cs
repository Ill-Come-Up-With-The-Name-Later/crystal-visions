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
    
    public float CurrentHealth { get => _enemy.Health.CurrentHealth; }
    public float MaxHealth { get => _enemy.Health.MaxHealth; }
    
    private EntityHealth Health { get => _enemy.Health; }

    public event Action<float> OnPatienceChange;
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
            GameManager.Instance.Player.Health.TakeDamage(2_147_483_647, gameObject.GetComponent<Enemy>());
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        OnPatienceChange -= GainStatBoosts;
        Health.OnHealthChanges -= TriggerHealthChangeEvent;
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
