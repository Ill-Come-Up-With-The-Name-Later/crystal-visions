using UnityEngine;

public class HealthBillboard : MonoBehaviour
{
    private Transform _player;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + _player.rotation * Vector3.forward,
            _player.rotation * Vector3.up);
    }
}
