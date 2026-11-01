using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAssist : MonoBehaviour
{
    [Header("Aim Assist Settings")]
    [SerializeField]
    private bool _active;
    
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
            return;
        
        if(_target != null)
            LockTo(_target);
        else
            FindTarget();
    }

    private void FindTarget()
    {
        List<Enemy> enemies = WaveManager.Instance.SpawnedEnemies();

        foreach (Enemy enemy in enemies)
        {
            if (PositionInFOV(enemy.GetComponent<Collider>().bounds.center) && 
                Vector3.Distance(enemy.transform.position, _player.transform.position) <= _range)
            {
                _target = enemy;
                break;
            }
        }
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
