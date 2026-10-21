using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
    
        private Vector3 _moveDirection;
    
        void Start()
        {
        
        }

        void Update()
        {
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            gameObject.transform.position += Vector3.Normalize(_moveDirection) * _speed * Time.deltaTime;
        }
    }

}    
