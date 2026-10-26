using UnityEngine;

[CreateAssetMenu(menuName="StateMachine/States/Attack State")]
public class AttackState : State
{
    public override void OnEnter(StateMachine stateMachine, State nextState)
    {
    }

    public override void OnExit(StateMachine stateMachine, State nextState)
    {
    }

    public override void FixedTick(StateMachine stateMachine, float deltaTime)
    {
        Entity owner = stateMachine.Owner;
        
        if (owner == null)
            return;

        if (owner is Enemy enemy)
        {
            enemy.transform.LookAt(enemy.Target.transform);
            
            if(!enemy.Target.Health.Invulnerable)
                enemy.UseRandomAttack();
        }
    }

    public override void Tick(StateMachine stateMachine, float deltaTime)
    {
    }
}
