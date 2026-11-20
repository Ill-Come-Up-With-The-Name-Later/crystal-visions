using System;
using UnityEngine;

public class CaligneaBoundary : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private float _bonusDrain;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private CaligneaBoss _boss;

    [SerializeField]
    private Collider _collider;

    private void Awake()
    {
        _boss = GetComponentInParent<CaligneaBoss>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Hurtbox"))
            return;
        
        Hurtbox otherHurtbox = other.gameObject.GetComponentInChildren<Hurtbox>();
        
        if (otherHurtbox == null)
            return;
        
        Entity target = otherHurtbox.Parent;

        if(target is Player)
            _boss.CurrentPatience = Math.Max(_boss.CurrentPatience - (_bonusDrain * Time.deltaTime), 0);
    }
}