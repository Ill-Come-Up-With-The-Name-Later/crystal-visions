using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private Player _player;

    [Header("Camera Settings")] 
    [SerializeField]
    private float _mouseSensitivity;

    [SerializeField] 
    private float _maxAngle;
    
    private float _xRotation = 0f;
    
    void Start()
    {
        _player = GetComponentInParent<Player>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void SetAimPitch(float pitch, float maxDelta)
    {
        pitch = Mathf.Clamp(pitch, -_maxAngle, _maxAngle);
        _xRotation = Mathf.MoveTowardsAngle(_xRotation, pitch, maxDelta);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
    
    private void Update()
    {
        if (_player.GetComponent<PlayerAimAssist>() != null)
        {
            PlayerAimAssist aimAssist = _player.GetComponent<PlayerAimAssist>();
            
            if (aimAssist.Target != null)
                return;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        
        _player.transform.Rotate(Vector3.up * mouseX);
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_maxAngle, _maxAngle);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}
