using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    private Vector3 _moveDirection;
    
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(0, 0, 0);
        
        _moveDirection = gameObject.transform.forward * Input.GetAxis("Vertical")
                         + gameObject.transform.right * Input.GetAxis("Horizontal");
        
        _rigidbody.MovePosition(_rigidbody.position + 
                                (Vector3.Normalize(_moveDirection) * _speed) * Time.deltaTime);
    }

    void Update()
    {
    }
}