using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Vector3 _moveDirection;
    
    [SerializeField]
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        _moveDirection = gameObject.transform.forward * Input.GetAxis("Vertical")
                         + gameObject.transform.right * Input.GetAxis("Horizontal");
        
        //Debug.DrawRay(gameObject.transform.position, _moveDirection, Color.red);
        
        _rigidbody.MovePosition(_rigidbody.position + 
                                (Vector3.Normalize(_moveDirection) * _player.SpeedStat.Value) * Time.deltaTime);
    }

    void Update()
    {
    }
}