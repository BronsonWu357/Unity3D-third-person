using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public StateMachine<EnemyController> StateMachine {  get; private set; }

    public State<EnemyController> IdleState {  get; private set; }

    public State<EnemyController> ChaseState {  get; private set; }

    public State<EnemyController> AttackState { get; private set; }

    public NavMeshAgent NavMeshAgent { get; private set; }

    public Animator animator { get; private set; }


    public void Start()
    {
        //用构造函数初始化
        StateMachine = new StateMachine<EnemyController>(this);

        IdleState = GetComponent<IdleState>();

        ChaseState = GetComponent<ChaseState>();

        StateMachine.ChangeState(IdleState);

        NavMeshAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        AttackState = GetComponent<AttackState>();
    }


    //每帧调用状态持续函数
    public void Update()
    {
        StateMachine.Execute();
    }
}
