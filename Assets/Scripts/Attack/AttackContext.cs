using UnityEngine;

public class AttackContext
{
    private Vector3 _forward;
    
    public Vector3 Forward { get { return _forward; } set { _forward = value; } }

    private Vector3 _positionOverride;
    
    public Vector3 Position { get { return _positionOverride; } set { _positionOverride = value; } }

    public AttackContext(Vector3 forward, Vector3 position)
    {
        _forward = forward;
        _positionOverride = position;
    }
}