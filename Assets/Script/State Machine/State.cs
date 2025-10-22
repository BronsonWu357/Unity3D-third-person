using UnityEngine;


//״̬�Ļ���
public class State<T> : MonoBehaviour
{
    //���뺯��
    public virtual void Enter(T owner) { }


    //���Ĵ�����
    public virtual void Execute() { }
    

    //�˳�����
    public virtual void Exit() { }
}
