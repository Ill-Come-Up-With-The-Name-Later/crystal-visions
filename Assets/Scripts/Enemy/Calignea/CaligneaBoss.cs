using System;
using UnityEngine;

public class CaligneaBoss : MonoBehaviour
{
    [Header("Calignea Specials")]
    [SerializeField]
    private float _maxPatience;

    [SerializeField]
    private float _patienceDrainRate;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private float _currentPatience;
    
    public float CurrentPatience { get => _currentPatience; set => _currentPatience = value; }

    [SerializeField] 
    private Enemy _enemy;

    public event Action<float> OnPatienceChange;
    public event Action OnDeath;
    public static event Action<CaligneaBoss> OnBossSpawned;

    private void Awake()
    {
        _enemy = gameObject.GetComponent<Enemy>();
    }

    void Start()
    {
        _currentPatience = _maxPatience;
        
        OnPatienceChange += GainStatBoosts;
        OnBossSpawned?.Invoke(this);
    }
    
    void Update()
    {
        _currentPatience = Mathf.Max(_currentPatience - (_patienceDrainRate * Time.deltaTime), 0);
        OnPatienceChange?.Invoke(_currentPatience);

        if (_currentPatience <= 0)
            GameManager.Instance.Player.Health.TakeDamage(2_147_483_647, gameObject.GetComponent<Enemy>());
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        OnPatienceChange -= GainStatBoosts;
    }

    private void GainStatBoosts(float currentPatience)
    {
        float patiencePercent = _currentPatience / _maxPatience;
        
        _enemy.SpeedStat.BonusValue = (1f - patiencePercent) * 5;
    }
}
