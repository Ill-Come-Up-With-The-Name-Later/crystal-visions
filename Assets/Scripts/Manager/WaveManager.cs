using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartWave();
    }
    
    public void StartWave()
    {
        EnemyWave wave = _waves[_currentWave];
        _enemiesLeftInWave = Random.Range(wave.MinEnemies, wave.MaxEnemies + 1);
        StartCoroutine(SpawnEnemies(wave));
    }

    IEnumerator SpawnEnemies(EnemyWave wave)
    {
        int spawned = 0;
        while (spawned < _enemiesLeftInWave)
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