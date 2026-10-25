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

    public static WaveManager Manager;

    private void Start()
    {
        Manager = this;
        EntityHealth.EntityDies += DecreaseEnemies;
        StartWave();
    }

    private void OnDestroy()
    {
        EntityHealth.EntityDies -= DecreaseEnemies;
    }

    public void StartWave()
    {
        EnemyWave wave = _waves[_currentWave];
        _enemiesLeftInWave = Random.Range(wave.MinEnemies, wave.MaxEnemies + 1);
        StartCoroutine(SpawnEnemies(wave));
    }

    void DecreaseEnemies(Entity entity)
    {
        if (entity is Enemy)
            _enemiesLeftInWave--;

        if (_enemiesLeftInWave <= 0)
        {
            _currentWave++;
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
                               new Vector3(Random.Range(-100, 100) + 50, 5, Random.Range(-100, 100) + 50);
            Instantiate(wave.GetRandomEnemy(), position, Quaternion.identity);
            spawned++;
            
            yield return new WaitForSeconds(
                Random.Range(wave.MinSpawnDelay, wave.MaxSpawnDelay));
        }
    }
}