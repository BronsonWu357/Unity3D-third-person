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
        //�ù��캯����ʼ��
        StateMachine = new StateMachine<EnemyController>(this);

        IdleState = GetComponent<IdleState>();

        ChaseState = GetComponent<ChaseState>();

        StateMachine.ChangeState(IdleState);

        NavMeshAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        AttackState = GetComponent<AttackState>();
    }


    //ÿ֡����״̬��������
    public void Update()
    {
        StateMachine.Execute();
    }
}
