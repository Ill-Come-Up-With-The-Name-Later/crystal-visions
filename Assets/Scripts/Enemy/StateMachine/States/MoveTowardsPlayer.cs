using UnityEngine;

[CreateAssetMenu(menuName="StateMachine/States/Move Towards Player")]
public class MoveTowardPlayer : State
{
    public override void OnEnter(StateMachine stateMachine, State previousState)
    {
    }

    public override void Tick(StateMachine stateMachine, float deltaTime)
    {
    }

    public override void FixedTick(StateMachine stateMachine, float deltaTime)
    {
        Entity owner = stateMachine.Owner;
        
        if (owner == null)
            return;

        if (owner is Enemy enemy)
        {
            Vector3 dir = enemy.PlayerPosition - enemy.transform.position;
            EnemyStats stats = enemy.Stats as EnemyStats;
            
            if(stats == null)
                return;

            if (Vector3.Distance(enemy.PlayerPosition, enemy.transform.position) > stats.MinRange)
            {
                enemy.Rigidbody.MovePosition(enemy.Rigidbody.position +
                                             Vector3.Normalize(dir) * enemy.Stats.speed * deltaTime);
                Quaternion rot = Quaternion.LookRotation(dir);
                enemy.transform.rotation = rot;
            }
        }
    }

    public override void OnExit(StateMachine stateMachine, State nextState)
    {
    }
}
