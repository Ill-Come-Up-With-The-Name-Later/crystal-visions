using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealNearbyEnemies : PeriodicAction
{
    [Header("Radius")]
    [SerializeField] 
    private float _healRadius;
    
    [Header("Heal Beam")]
    [SerializeField]
    private HealBeam _healBeam;

    [Header("Debug (Do not edit)")] 
    private Enemy _parent;

    protected override void Start()
    {
        base.Start();
        _parent = GetComponentInParent<Enemy>();
    }
    
    protected override void PerformAction()
    {
        List<Enemy> enemies = FindObjectsOfType<Enemy>().ToList().
            Where(enemy => Vector3.Distance(gameObject.transform.position, enemy.transform.position) 
                           < _healRadius && enemy != _parent).ToList();

        foreach (Enemy enemy in enemies)
        {
            HealBeam beam = Instantiate(_healBeam);

            beam.Source = _parent;
            beam.Target = enemy;
            beam.HealAmount = _parent.DamageStat.Value * 2;
            
            beam.Heal();
        }
    }
}