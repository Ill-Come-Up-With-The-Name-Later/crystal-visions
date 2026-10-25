using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private bool Lost;
    
    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Crystal _crystal;

    public event Action OnLoss;

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
    }

    void Start()
    {
        _player.OnPlayerDeath += SetLost;
        _crystal.Health.OnDeath += SetLost;
    }
    
    void Update()
    {
        
    }

    void SetLost()
    {
        Debug.Log("Player has lost");
        Lost = true;
        OnLoss?.Invoke();
        Time.timeScale = 0;
    }
}
