using UnityEngine;


//状态的基类
public class State<T> : MonoBehaviour
{
    //进入函数
    public virtual void Enter(T owner) { }


    //核心处理函数
    public virtual void Execute() { }
    

    //退出函数
    public virtual void Exit() { }
}
