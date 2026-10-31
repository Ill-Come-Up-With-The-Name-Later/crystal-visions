using System;
using UnityEngine;

[Serializable]
public struct EntityStat
{
    [SerializeField]
    private float _baseValue;
    
    [SerializeField]
    private float _bonusValue;
    
    [SerializeField]
    private float _multiplier;
    
    public float BaseValue { get { return _baseValue; } set { _baseValue = value; } }
    public float BonusValue { get { return _bonusValue; } set { _bonusValue = value; } }
    public float Multiplier { get { return _multiplier; } set { _multiplier = value; } }
    public float Value { get { return (_baseValue + _bonusValue) * (1 + _multiplier); } }
}
