using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : State<EnemyController>
{
    private EnemyController enemy;

    [SerializeField] VisionSensor visionSensor;

    [SerializeField] private float distanceToStand = 1f;

    //��Ŀ��λ�õĵ�ǰ����
    private float currentDistance;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavMeshAgent.stoppingDistance = distanceToStand;
    }


    public override void Execute()
    {
        //�Զ�Ѱ·����
        enemy.NavMeshAgent.SetDestination(visionSensor.Target.transform.position);


        //enemy.NavMeshAgent.velocityΪ��ǰ�ٶȣ�enemy.NavMeshAgent.speedΪ����ٶ�
        enemy.animator.SetFloat("moveAmount",enemy.NavMeshAgent.velocity.magnitude / enemy.NavMeshAgent.speed);

        currentDistance = Vector3.Distance(visionSensor.Target.transform.position, transform.position);

        //ת��Ϊ����״̬
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
