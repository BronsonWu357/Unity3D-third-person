using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : State<EnemyController>
{
    private EnemyController enemy;

    [SerializeField] VisionSensor visionSensor;

    [SerializeField] private float distanceToStand = 1f;

    //离目标位置的当前距离
    private float currentDistance;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavMeshAgent.stoppingDistance = distanceToStand;
    }


    public override void Execute()
    {
        //自动寻路函数
        enemy.NavMeshAgent.SetDestination(visionSensor.Target.transform.position);


        //enemy.NavMeshAgent.velocity为当前速度，enemy.NavMeshAgent.speed为最大速度
        enemy.animator.SetFloat("moveAmount",enemy.NavMeshAgent.velocity.magnitude / enemy.NavMeshAgent.speed);

        currentDistance = Vector3.Distance(visionSensor.Target.transform.position, transform.position);

        //转换为攻击状态
        if (currentDistance <= distanceToStand)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);

            enemy.animator.SetFloat("moveAmount", 0);
        }


    }


    public override void Exit()
    {
        Debug.Log("Exit ChaseState");
    }
}
