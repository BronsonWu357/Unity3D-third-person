using UnityEngine;

//EnemyController可以确保状态只能用于 EnemyController 类型
public class PainState : State<EnemyController>
{
    //定义敌人目标
    private EnemyController enemy;


    [SerializeField] private VisionSensor VisionSensor;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }
}
