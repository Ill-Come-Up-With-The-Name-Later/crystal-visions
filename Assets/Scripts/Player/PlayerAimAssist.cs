using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAimAssist : MonoBehaviour
{
    [Header("Aim Assist Settings")]
    [SerializeField]
    private bool _active;
    
    public bool Active { get => _active; set => _active = value; }
    
    [SerializeField]
    private float _fov;

    [Range(0f, 1f)]
    [SerializeField]
    private float _strength;
    
    [SerializeField]
    private float _range;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Entity _target;
    
    public Entity Target { get => _target; }

    void Start()
    {
        _player = GameManager.Instance.Player;
    }

    void Update()
    {
        if (!_active)
        {
            _target = null;
            return;
        }

        if(_target != null)
            LockTo(_target);
        else
            FindTarget();
    }

    private void FindTarget()
    {
        List<Enemy> enemies = WaveManager.Instance.SpawnedEnemies();
        List<Enemy> enemiesEligible = enemies.Where(x => PositionInFOV(x.GetComponent<Collider>().bounds.center)
                                                         && Vector3.Distance(x.GetComponent<Collider>().bounds.center,
                                                             _player.transform.position) <= _range).ToList();

        // Closest enemy to the player
        _target = enemiesEligible.Count == 0 ? null
            : enemiesEligible.Where(t => t != null)
                .OrderBy(t => (t.transform.position - _player.transform.position).sqrMagnitude)
                .FirstOrDefault();
    }

    private void LockTo(Entity target)
    {
        Vector3 targetPos = target.GetComponent<Collider>().bounds.center;
        Vector3 direction = targetPos - _player.transform.position;
        
        if (PositionInFOV(targetPos))
        {
            // Smooth rotation to target
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            Quaternion horizontalRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation,
                horizontalRotation, _strength * Time.deltaTime);
            
            float targetPitch = targetRotation.eulerAngles.x;

            if (targetPitch > 180f)
                targetPitch -= 360f;
            
            _player.transform.rotation = Quaternion.RotateTowards(
                _player.transform.rotation, horizontalRotation, 
                _strength * 90 * Time.deltaTime);

            PlayerCamera camera = _player.GetComponentInChildren<PlayerCamera>();

            if (camera != null)
                camera.SetAimPitch(targetPitch, _strength * 90);
        }
        else
            _target = null;
    }

    private bool PositionInFOV(Vector3 pos)
    {
        Vector3 forward = _player.transform.forward;
        Vector3 toPos = (pos - _player.transform.position).normalized;
        
        float dot = Vector3.Dot(forward, toPos);
        float halfFov = Mathf.Deg2Rad * _fov * 0.5f;
        float threshold = Mathf.Cos(halfFov);

        return dot >= threshold;
    }
}
