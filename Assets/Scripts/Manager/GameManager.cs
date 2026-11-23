using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private bool _lost;

    [SerializeField]
    private bool _won;
    
    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Crystal _crystal;

    [SerializeField]    
    private CaligneaBoss _boss;
    public Player Player => _player;
    
    public Crystal Crystal => _crystal;
    
    public CaligneaBoss Boss { get => _boss; set => _boss = value; }

    public event Action OnLoss;
    public event Action OnWin;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _won = false;
        _lost = false;
        _boss = null;
        
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
    }

    void Start()
    {
        _player.OnPlayerDeath += SetLost;
        _crystal.Health.OnDeath += SetLost;
        CaligneaBoss.OnBossSpawned += SetBoss;
        
        Time.timeScale = 1;
    }

    void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
        
        _player.OnPlayerDeath -= SetLost;
        _crystal.Health.OnDeath -= SetLost;
        CaligneaBoss.OnBossSpawned -= SetBoss;

        if (_boss != null)
            _boss.OnDeath -= SetWon;
    }
    
    void Update()
    {
        if (_lost || _won)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void SetBoss(CaligneaBoss boss)
    {
        _boss = boss;
        _boss.OnDeath += SetWon;
    }

    void ResetBoss()
    {
        _boss = null;
    }

    void SetLost()
    {
        //Debug.Log("Player has lost");
        _lost = true;
        OnLoss?.Invoke();
        Time.timeScale = 0;
    }
    
    public void SetWon()
    {
        //Debug.Log("Player has won");
        _won = true;
        OnWin?.Invoke();
        Time.timeScale = 0;
    }
}
