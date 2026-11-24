using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] 
    private List<EnemyWave> _waves;

    [Header("Spawning Particles")]
    [SerializeField]
    private ParticleSystem _spawnParticles;

    [Header("Debug (Do not edit)")] 
    [SerializeField] [Range(0, 4)]
    private int _startingWave = 0;
    
    [SerializeField]
    private int _currentWave;

    [SerializeField]
    private int _enemiesLeftInWave;

    public static WaveManager Instance;

    private bool _spawning;
    private bool _finishedSpawning;
    
    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveEnded;
    public event Action<int> ChangeEnemiesLeft;
    
    public int CurrentWave { get => _currentWave; }
    
    public int Waves { get { return _waves.Count; } }
    
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
        EntityHealth.EntityDies += DecreaseEnemies;
        OnWaveEnded += StartWave;
        
        _currentWave = _startingWave;
        _enemiesLeftInWave = 0;
        _spawning = false;
        _finishedSpawning = false;
        
        StartWave();
    }

    private void OnDestroy()
    {
        EntityHealth.EntityDies -= DecreaseEnemies;
    }

    public void StartWave(int _ = 0)
    {
        if (_currentWave >= _waves.Count)
            return;

        EnemyWave wave = _waves[_currentWave];
        _enemiesLeftInWave = Random.Range(wave.MinEnemies, wave.MaxEnemies + 1) + wave.GuaranteedSpawns.Count;
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

        if (_enemiesLeftInWave <= 0 && !_spawning && _finishedSpawning)
        {
            _currentWave++;
            OnWaveEnded?.Invoke(_currentWave);
        }
    }

    public List<Enemy> SpawnedEnemies()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        return enemies.ToList();
    }

    IEnumerator SpawnEnemies(EnemyWave wave)
    {
        int spawned = 0;
        int enemiesToSpawn = _enemiesLeftInWave;
        _spawning = true;
        _finishedSpawning = false;
        
        while (spawned < enemiesToSpawn - wave.GuaranteedSpawns.Count)
        {
            yield return new WaitForSeconds(Random.Range(wave.MinSpawnDelay, wave.MaxSpawnDelay));
            
            Vector3 position = transform.position + 
                               new Vector3(Random.Range(-30, 30) + 25, 3.5f, Random.Range(30, 30) + 25);
            SpawnEnemy(wave.GetRandomEnemy(), position);
            spawned++;
        }

        if (wave.GuaranteedSpawns.Count == 0)
        {
            _spawning = false;
            _finishedSpawning = true;
            yield break;
        }
        
        yield return new WaitForSeconds(wave.GuaranteedSpawnDelay);

        foreach (Enemy enemy in wave.GuaranteedSpawns)
        {
            Vector3 position = transform.position + 
                               new Vector3(Random.Range(-30, 30) + 25, 3.5f, Random.Range(30, 30) + 25);
            SpawnEnemy(enemy, position);
            spawned++;
            
            yield return new WaitForSeconds(wave.GuaranteedSpawnDelay);
        }
        
        _spawning = false;
        _finishedSpawning = true;
    }

    void SpawnEnemy(Enemy enemy, Vector3 position)
    {
        Instantiate(_spawnParticles, position, Quaternion.identity);
        Instantiate(enemy, position, Quaternion.identity);
    }
}