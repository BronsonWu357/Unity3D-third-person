using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Game.CameraSystem;

public class PlayerController : MonoBehaviour
{
    //速度
    [SerializeField] private float speed = 5.0f;

    //获取镜头对象
    [SerializeField] private CameraController cameraController;

    //目标旋转方向
    public Quaternion targetRotation {  get; private set; }

    //角色旋转速度
    [SerializeField] float rotationSpeed = 500f;

    //角色动画控制
    private Animator animator;

    //角色控制
    private CharacterController characterController;

    //落地碰撞区域偏移
    [SerializeField] private Vector3 groundCheckOffset;

    //落地碰撞区域半径
    private float groundCheckRadius = 0.2f;

    //地面层
    private LayerMask groundLayer;

    //y向量速度
    private float ySpeed;

    private MeleeFighter meleeFighter;

    //衣服鞋子动画组件
    [SerializeField] Animator outwearAnimator;
    [SerializeField] Animator shoesAnimator;

    public bool canMove = true;


    // 初始化代码在这里执行
    // 在对象创建时自动调用
    public void Awake()
    {
        // 获取主相机的 CameraController 组件并赋值给变量
        //cameraController = Camera.main.GetComponent<CameraController>();

        //在当前游戏对象上查找组件
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();

        meleeFighter = GetComponent<MeleeFighter>();
    }



    //主函数，每帧调用
    public void Update()
    {
        //如果在攻击状态下，不移动
        if (meleeFighter.InAction)
        {
            //重置动画变量，使得攻击完后会向待机动画过渡
            animator.SetFloat("moveAmount", 0);

            outwearAnimator.SetFloat("moveAmount", 0);

            shoesAnimator.SetFloat("moveAmount", 0);

            return;
        }

        float h = 0;
        float v = 0;
        if (canMove)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }


        //面向方向
        var rotation = cameraController.PlanerRotation;
        //移动输入
        var moveInput = (new Vector3(h, 0, v)).normalized;
        //移动方向
        var moveDirection = rotation * moveInput;
        //角色实际移动速度
        var velocity = moveDirection * speed;
        //Mathf.Abs求绝对值
        //Clamp01可以将值限制在0到1之间
        var moveAmout = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));


        //自由落体
        FreeFall(velocity);

        //移动
        CharacterMove(velocity);

        CharacterRotate(moveAmout, moveDirection);
    }


    //触地检查
    public bool GroudCheck()
    {
        //判断检查区域是否和地面层发生碰撞
       return Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }


    //绘制触地检查区域
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius);
    }


    //角色移动
    public void CharacterMove(Vector3 velocity)
    {
        //使用CharacterController移动（无惯性）
        //角色移动执行代码
        characterController.Move(velocity * Time.deltaTime);
    }


    //自由落体
    public void FreeFall(Vector3 velocity)
    {

        if (GroudCheck())
        {
            velocity.y = -0.5f;
        }
        else
        {
            velocity.y += Physics.gravity.y;
        }
    }


    //角色旋转,并传递动画输入参数
    public void CharacterRotate(float moveAmout,Vector3 moveDirection)
    {
        //当运动输入大于零时调用
        if (moveAmout > 0)
        {
            //乘以deltaTime可以避免帧率实际运动的冲突
            //transform.position += moveDirection * speed * Time.deltaTime;

            //更新角色的旋转角度，使其等于移动方向
            targetRotation = Quaternion.LookRotation(moveDirection);

            //平滑旋转
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //将位移量传递给动画树
        animator.SetFloat("moveAmount", moveAmout, 0.2f, Time.deltaTime);
        outwearAnimator.SetFloat("moveAmount", moveAmout, 0.2f, Time.deltaTime);
        shoesAnimator.SetFloat("moveAmount", moveAmout, 0.2f, Time.deltaTime);
    }


}
