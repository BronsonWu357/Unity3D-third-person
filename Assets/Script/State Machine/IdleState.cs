using UnityEngine;

public class IdleState : State<EnemyController>
{
    //�������Ŀ��
    private EnemyController enemy;


    [SerializeField] private VisionSensor VisionSensor;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }


    public override void Execute()
    {
        foreach(var target in VisionSensor.TargetInRange)
        {
            //��ǰ����Ҫ׷�ٵ�Ŀ���������
            var vectorToTarget = target.transform.position - transform.position;
            //��ǰ������������Ŀ����������н�
            float angle = Vector3.Angle(transform.forward, vectorToTarget);

            //С����Ұ�Ƕ������׷��״̬
            if(angle < VisionSensor.FieldOfView / 2)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
                VisionSensor.Target = target;
                break;
            }
        }
    }


    public override void Exit()
    {

    }
}
