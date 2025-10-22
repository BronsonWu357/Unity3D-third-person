using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeFighter : MonoBehaviour
{
    Animator animator;

    [SerializeField] Animator outwearAnimator;

    [SerializeField] Animator shoesAnimator;

    [SerializeField] private PlayerController playerController;

    public enum AttackState { Idle,WindUp,Impact,CoolDown }

    //目标对象
    [SerializeField] GameObject puch;

    private BoxCollider puchCollider;



    //是否在执行动画
    //设置属性
    public bool InAction { get; private set; } = false;

    public AttackState attackState;


    public void Awake()
    {
        animator = GetComponent<Animator>();

        attackState = AttackState.Idle;
    }

    //在awake（）之后调用，擅长处理对象间的依赖
    public void Start()
    {
        if (puch != null) 
        {
            puchCollider = puch.GetComponent<BoxCollider>();

            puchCollider.enabled = false;
        }
    }


    public void TryToAttack()
    {
        if (!InAction)
        {
            //启动协程函数
            StartCoroutine(Attack("Jab Cross"));
        }
    }


    //IEnumerator为协程函数，相当于一个计时器，让我们等待若干时间后执行接下来的操作
    IEnumerator Attack(string AnimationName)
    {
        //切换为前摇状态
        attackState = AttackState.WindUp;

        InAction = true;

        //CrossFade()相较于Play（），它可以让动作过度，使得动作更连贯
        //0.2f相当于当前动画的0.2倍时间，这个时间用来过度
        animator.CrossFade(AnimationName, 0.2f);

        if(outwearAnimator != null)
        {
            outwearAnimator.CrossFade(AnimationName, 0.2f);
        }

        if (shoesAnimator != null)
        {
            shoesAnimator.CrossFade(AnimationName, 0.2f);
        }

        //等待一帧,让动画进入过渡状态
        yield return null;

        //当前状态为过度状态，用GetNextAnimatorStateInfo()，1代表第1个索引（从零开始数）
        //获取下一个动画
        var animationState = animator.GetNextAnimatorStateInfo(1);

        float timer = 0f;

        float impactStartTime = 0.53f;

        float impactEndTime = 0.75f;

        while (timer <= animationState.length)
        {
            //每次循环加一帧
            timer += Time.deltaTime;

            //根据不同状态执行不同代码
            if (attackState == AttackState.WindUp)
            {
                if (timer >= impactStartTime)
                {
                    attackState = AttackState.Impact;
                    puchCollider.enabled = true;
                }
            }
            else if (attackState == AttackState.Impact)
            {
                if (timer >= impactEndTime)
                {
                    attackState = AttackState.CoolDown;
                    puchCollider.enabled = false;
                }
            }
            else if (attackState == AttackState.CoolDown)
            {

            }

            yield return null;
        }

        attackState = AttackState.Idle;

        InAction = false;
    }


    //发生碰撞自动触发函数
    public void OnTriggerEnter(Collider other)
    {
        //二次判断是否为hitBox
        if (other.tag == "HitBox" && !InAction)
        {
            StartCoroutine(HurtAnimation("Impact"));
        }
    }

    //受击动画
    IEnumerator HurtAnimation(string AnimationName)
    {
        if(playerController != null)
        {
            //角色进入受击状态
            playerController.canMove = false;
        }

        animator.SetLayerWeight(1, 1f);

        //CrossFade()相较于Play（），它可以让动作过度，使得动作更连贯
        //0.2f相当于当前动画的0.2倍时间，这个时间用来过度
        animator.CrossFade(AnimationName, 0.2f);

        if (outwearAnimator != null && shoesAnimator != null)
        {
            outwearAnimator.SetLayerWeight(1, 1f);
            outwearAnimator.CrossFade(AnimationName, 0.2f);

            shoesAnimator.SetLayerWeight(1, 1f);
            shoesAnimator.CrossFade(AnimationName, 0.2f);
        }
        

        //等待一帧,让动画进入过渡状态
        yield return null;

        //当前状态为过度状态，用GetNextAnimatorStateInfo()，1代表第1个索引（从零开始数）
        //获取下一个动画
        var animationState = animator.GetNextAnimatorStateInfo(1);

        //等待当前动画时间过半
        yield return new WaitForSeconds(animationState.length * 3 / 10);

        float timer = 0f;

        while (timer <= animationState.length * 7 / 10)
        {
            //每次循环加一帧
            timer += Time.deltaTime;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (h != 0 || v != 0)
            {
                //将动画的第0层优先级设为1，第一层设为0
                animator.SetLayerWeight(0, 1f);
                animator.SetLayerWeight(1, 0f);


                if (outwearAnimator != null && shoesAnimator != null)
                {
                    outwearAnimator.SetLayerWeight(0, 1f);
                    outwearAnimator.SetLayerWeight(1, 0f);

                    shoesAnimator.SetLayerWeight(0, 1f);
                    shoesAnimator.SetLayerWeight(1, 0f);
                }

                if (playerController != null)
                {
                    playerController.canMove = true;
                }

                break;
            }

            //等待一帧
            yield return null;
        }

        //再等待剩余时间
        yield return new WaitForSeconds(animationState.length * 7 / 10 - timer);

        if (playerController != null)
        {
            playerController.canMove = true;
        }

        animator.SetLayerWeight(1, 1f);

        if (outwearAnimator != null && shoesAnimator != null)
        {
            outwearAnimator.SetLayerWeight(1, 1f);
            shoesAnimator.SetLayerWeight(1, 1f);
        }
    }

}
