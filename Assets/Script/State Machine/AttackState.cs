using UnityEngine;

public class AttackState : State<EnemyController>
{
    private EnemyController enemy;

    private MeleeFighter meleeFighter;

    private float currentDistance;

    [SerializeField] VisionSensor visionSensor;

    [SerializeField] private float distanceToStand = 1f;

    [SerializeField] private float attackSpeed = 1f;

    //¼ÆÊ±Æ÷
    private float timer = 0f;

    public void Start()
    {
        meleeFighter = GetComponent<MeleeFighter>();
    }

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }


    public override void Execute()
    {
        Vector3 direction = visionSensor.Target.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);

        if (timer <= 0)
        {
            timer = attackSpeed;

            currentDistance = Vector3.Distance(visionSensor.Target.transform.position, transform.position);

            if (currentDistance <= distanceToStand)
            {
                meleeFighter.TryToAttack();
            }
            else
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);

                timer = 0f;
            }
        }

        timer -= Time.deltaTime;
    }


    public override void Exit()
    {

    }

}
