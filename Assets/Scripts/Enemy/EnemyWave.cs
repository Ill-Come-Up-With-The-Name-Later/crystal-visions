using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeightedEnemy
{
    [SerializeField]
    private Enemy _enemy;
    
    [SerializeField]
    private int _weight;
    
    public Enemy Enemy
    {
        get { return _enemy; }
    }
    
    public int Weight 
    {
        get { return _weight; }
    }
}

[CreateAssetMenu(menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private List<WeightedEnemy> _enemies;
    
    public IReadOnlyList<WeightedEnemy> Enemies { get => _enemies; }
    
    [SerializeField]
    private int _minEnemies;
    
    public int MinEnemies { get => _minEnemies; }
    
    [SerializeField]
    private int _maxEnemies;
    
    public int MaxEnemies { get => _maxEnemies; }

    [SerializeField]
    private int _minSpawnDelay;
    
    public int MinSpawnDelay { get => _minSpawnDelay; }
    
    [SerializeField]
    private int _maxSpawnDelay;
    
    public int MaxSpawnDelay { get => _maxSpawnDelay; }

    public Enemy GetRandomEnemy()
    {
        ItemSelector<Enemy> enemies = new ItemSelector<Enemy>();
        
        foreach(WeightedEnemy enemy in _enemies)
            enemies.AddItem(enemy.Enemy, enemy.Weight);

        return enemies.Roll();
    }
}