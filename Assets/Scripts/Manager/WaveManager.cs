using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] 
    private List<EnemyWave> _waves;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private int _currentWave;

    [SerializeField]
    private int _enemiesLeftInWave;

    public static WaveManager Instance;
    
    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveEnded;
    public event Action<int> ChangeEnemiesLeft;
    
    public int CurrentWave { get => _currentWave; }
    
    public int EnemiesLeftInWave { get => _enemiesLeftInWave; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _currentWave = 0;
        _enemiesLeftInWave = 0;
        
        EntityHealth.EntityDies += DecreaseEnemies;
        StartWave();
    }

    private void OnDestroy()
    {
        EntityHealth.EntityDies -= DecreaseEnemies;
    }

    public void StartWave()
    {
        if (_currentWave >= _waves.Count)
        {
            GameManager.Instance.SetWon();
            return;
        }

        EnemyWave wave = _waves[_currentWave];
        _enemiesLeftInWave = Random.Range(wave.MinEnemies, wave.MaxEnemies + 1);
        ChangeEnemiesLeft?.Invoke(_enemiesLeftInWave);
        
        StartCoroutine(SpawnEnemies(wave));
        OnWaveStarted?.Invoke(_currentWave);
    }

    void DecreaseEnemies(Entity entity)
    {
        if (entity is Enemy)
        {
            _enemiesLeftInWave--;
            ChangeEnemiesLeft?.Invoke(_enemiesLeftInWave);
        }

        if (_enemiesLeftInWave <= 0)
        {
            _currentWave++;
            OnWaveEnded?.Invoke(_currentWave);
            StartWave();
        }
    }

    IEnumerator SpawnEnemies(EnemyWave wave)
    {
        int spawned = 0;
        int enemiesToSpawn = _enemiesLeftInWave;
        
        while (spawned < enemiesToSpawn)
        {
            Vector3 position = transform.position + 
                               new Vector3(Random.Range(-40, 40) + 50, 2, Random.Range(-40, 40) + 20);
            Instantiate(wave.GetRandomEnemy(), position, Quaternion.identity);
            spawned++;
            
            yield return new WaitForSeconds(
                Random.Range(wave.MinSpawnDelay, wave.MaxSpawnDelay));
        }
    }
}