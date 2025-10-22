using UnityEngine;

public class IdleState : State<EnemyController>
{
    //定义敌人目标
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
            //当前对象到要追踪的目标对象向量
            var vectorToTarget = target.transform.position - transform.position;
            //当前对象面向方向于目标对象向量夹角
            float angle = Vector3.Angle(transform.forward, vectorToTarget);

            //小于视野角度则进入追踪状态
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
