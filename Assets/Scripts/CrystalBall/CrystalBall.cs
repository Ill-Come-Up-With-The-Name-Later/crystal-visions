using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

enum MouseButton
{
    Left,
    Right,
    Middle
}

[Serializable]
struct AttackMode
{
    [Header("Attacks")]
    [SerializeField]
    private AttackAndCooldown _primaryAttack;
    
    [SerializeField]
    private AttackAndCooldown _secondaryAttack;
    
    public AttackAndCooldown Primary { get { return _primaryAttack; } }
    
    public AttackAndCooldown Secondary { get { return _secondaryAttack; } }
}

public class CrystalBall : MonoBehaviour
{
    [Header("Crystal Ball Attack Fail Particle")]
    [SerializeField]
    private ParticleSystem _failParticle;

    [Header("Crystal Ball Attacks")]
    [SerializeField]
    private List<AttackMode> _attackModes;

    [SerializeField]
    private List<Color> _modeColors;

    [SerializeField]
    private float _colorIntensity;
        
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _owner;

    [SerializeField]
    private LineRenderer _laserRenderer;
    
    public LineRenderer Laser { get { return _laserRenderer; } }
    
    [SerializeField]
    private AttackMode _current;
    private int _currentIndex;

    [SerializeField]
    private VisualEffect _effect;

    private ParticleSystem _modeSwitchEffect;

    private Dictionary<Attack, float> _attackCooldowns = new Dictionary<Attack, float>(); // This isn't great, but it works

    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
        
        if(_owner == null)
            Debug.LogError("No owning player found");

        _effect = GetComponentInChildren<VisualEffect>();
        
        if(_effect == null)
            Debug.LogError("No effect found");
        
        _laserRenderer = GetComponentInChildren<LineRenderer>();
        
        if(_laserRenderer == null)
            Debug.LogError("No line renderer found");
        
        _modeSwitchEffect = GetComponentInChildren<ParticleSystem>();
        
        if(_modeSwitchEffect == null)
            Debug.LogError("No mode switch particle system found");
    }

    void Start()
    {
        _currentIndex = 0;
        _current = _attackModes[_currentIndex];
        
        foreach (AttackMode mode in _attackModes)
        {
            _attackCooldowns[mode.Primary.Attack] = 0;
            _attackCooldowns[mode.Secondary.Attack] = 0;
        }
        
        if (_effect.HasVector4("color"))
            _effect.SetVector4("color", _modeColors[_currentIndex] * _colorIntensity);
        
        if (_effect.HasVector4("flashColor"))
            _effect.SetVector4("flashColor", _modeColors[_currentIndex] * _colorIntensity);
    }
    
    void Update()
    {
        List<Attack> attacks = _attackCooldowns.Keys.ToList();

        foreach (Attack attack in attacks)
            _attackCooldowns[attack] -= Time.deltaTime;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
            UseAttack(_current.Primary);

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
            UseAttack(_current.Secondary);

        if (Input.GetMouseButtonDown((int)MouseButton.Middle))
        {
            _currentIndex = (_currentIndex + 1) % _attackModes.Count;
            _current = _attackModes[_currentIndex];
            
            _modeSwitchEffect.Stop();
            _modeSwitchEffect.Play();

            if (_effect.HasVector4("color"))
                _effect.SetVector4("color", _modeColors[_currentIndex] * _colorIntensity);
            
            if (_effect.HasVector4("flashColor"))
                _effect.SetVector4("flashColor", _modeColors[_currentIndex] * _colorIntensity);
        }
    }

    public void UseAttack(AttackAndCooldown attack)
    {
        if (!CanUseAttack(attack.Attack))
        {
            Instantiate(_failParticle, transform.position, Quaternion.identity);
            return;
        }

        AttackContext context = new AttackContext();
        context.Forward = _owner.Camera.transform.forward;
        attack.Attack.PerformAttack(_owner, context);
        _attackCooldowns[attack.Attack] = attack.Cooldown;
    }

    public bool CanUseAttack(Attack attack)
    {
        _attackCooldowns.TryGetValue(attack, out var cooldown);
        return cooldown <= 0;
    }

    public void HideLaserCoroutine()
    {
        StartCoroutine(HideLaser());
    }
    
    IEnumerator HideLaser()
    {
        yield return new WaitForSeconds(0.5f);
        _laserRenderer.enabled = false;
    }
}
