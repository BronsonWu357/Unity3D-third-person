using UnityEngine;

//EnemyController����ȷ��״ֻ̬������ EnemyController ����
public class PainState : State<EnemyController>
{
    //�������Ŀ��
    private EnemyController enemy;


    [SerializeField] private VisionSensor VisionSensor;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }
}
