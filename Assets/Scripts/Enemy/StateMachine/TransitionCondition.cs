using System;
using UnityEngine;

[Serializable]
public abstract class TransitionCondition : MonoBehaviour
{
    public abstract bool Evaluate(Entity entity, StateMachine stateMachine);
}
