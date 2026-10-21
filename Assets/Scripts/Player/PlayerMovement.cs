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
            _moveDirection = gameObject.transform.forward * Input.GetAxis("Vertical")
                             + gameObject.transform.right * Input.GetAxis("Horizontal");

            gameObject.transform.position += Vector3.Normalize(_moveDirection) * _speed * Time.deltaTime;
        }
    }

}    
