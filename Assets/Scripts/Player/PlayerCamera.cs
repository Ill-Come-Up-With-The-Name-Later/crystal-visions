using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private Transform _player;

    [Header("Camera Settings")] 
    [SerializeField]
    private float _mouseSensitivity = 550f;

    [SerializeField]
    private float _maxAngle = 85f;
    
    private float _xRotation = 0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_maxAngle, _maxAngle);
        
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _player.Rotate(Vector3.up * mouseX);
    }
}
