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

    //Ŀ�����
    [SerializeField] GameObject puch;

    private BoxCollider puchCollider;



    //�Ƿ���ִ�ж���
    //��������
    public bool InAction { get; private set; } = false;

    public AttackState attackState;


    public void Awake()
    {
        animator = GetComponent<Animator>();

        attackState = AttackState.Idle;
    }

    //��awake����֮����ã��ó��������������
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
            //����Э�̺���
            StartCoroutine(Attack("Jab Cross"));
        }
    }


    //IEnumeratorΪЭ�̺������൱��һ����ʱ���������ǵȴ�����ʱ���ִ�н������Ĳ���
    IEnumerator Attack(string AnimationName)
    {
        //�л�Ϊǰҡ״̬
        attackState = AttackState.WindUp;

        InAction = true;

        //CrossFade()�����Play�������������ö������ȣ�ʹ�ö���������
        //0.2f�൱�ڵ�ǰ������0.2��ʱ�䣬���ʱ����������
        animator.CrossFade(AnimationName, 0.2f);

        if(outwearAnimator != null)
        {
            outwearAnimator.CrossFade(AnimationName, 0.2f);
        }

        if (shoesAnimator != null)
        {
            shoesAnimator.CrossFade(AnimationName, 0.2f);
        }

        //�ȴ�һ֡,�ö����������״̬
        yield return null;

        //��ǰ״̬Ϊ����״̬����GetNextAnimatorStateInfo()��1�����1�����������㿪ʼ����
        //��ȡ��һ������
        var animationState = animator.GetNextAnimatorStateInfo(1);

        float timer = 0f;

        float impactStartTime = 0.53f;

        float impactEndTime = 0.75f;

        while (timer <= animationState.length)
        {
            //ÿ��ѭ����һ֡
            timer += Time.deltaTime;

            //���ݲ�ͬ״ִ̬�в�ͬ����
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


    //������ײ�Զ���������
    public void OnTriggerEnter(Collider other)
    {
        //�����ж��Ƿ�ΪhitBox
        if (other.tag == "HitBox" && !InAction)
        {
            StartCoroutine(HurtAnimation("Impact"));
        }
    }

    //�ܻ�����
    IEnumerator HurtAnimation(string AnimationName)
    {
        if(playerController != null)
        {
            //��ɫ�����ܻ�״̬
            playerController.canMove = false;
        }

        animator.SetLayerWeight(1, 1f);

        //CrossFade()�����Play�������������ö������ȣ�ʹ�ö���������
        //0.2f�൱�ڵ�ǰ������0.2��ʱ�䣬���ʱ����������
        animator.CrossFade(AnimationName, 0.2f);

        if (outwearAnimator != null && shoesAnimator != null)
        {
            outwearAnimator.SetLayerWeight(1, 1f);
            outwearAnimator.CrossFade(AnimationName, 0.2f);

            shoesAnimator.SetLayerWeight(1, 1f);
            shoesAnimator.CrossFade(AnimationName, 0.2f);
        }
        

        //�ȴ�һ֡,�ö����������״̬
        yield return null;

        //��ǰ״̬Ϊ����״̬����GetNextAnimatorStateInfo()��1�����1�����������㿪ʼ����
        //��ȡ��һ������
        var animationState = animator.GetNextAnimatorStateInfo(1);

        //�ȴ���ǰ����ʱ�����
        yield return new WaitForSeconds(animationState.length * 3 / 10);

        float timer = 0f;

        while (timer <= animationState.length * 7 / 10)
        {
            //ÿ��ѭ����һ֡
            timer += Time.deltaTime;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (h != 0 || v != 0)
            {
                //�������ĵ�0�����ȼ���Ϊ1����һ����Ϊ0
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

            //�ȴ�һ֡
            yield return null;
        }

        //�ٵȴ�ʣ��ʱ��
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
