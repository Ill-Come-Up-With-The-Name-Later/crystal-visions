using UnityEngine;

public class AttackContext
{
    private Vector3 _forward;
    
    public Vector3 Forward { get { return _forward; } set { _forward = value; } }

    private Vector3 positionOverride;
    
    public Vector3 Position { get { return positionOverride; } set { positionOverride = value; } }
}