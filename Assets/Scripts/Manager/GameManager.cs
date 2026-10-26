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
    
    public Player Player => _player;
    
    public Crystal Crystal => _crystal;

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
        
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
    }

    void Start()
    {
        _player.OnPlayerDeath += SetLost;
        _crystal.Health.OnDeath += SetLost;
    }

    void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
        
        _player.OnPlayerDeath -= SetLost;
        _crystal.Health.OnDeath -= SetLost;
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
