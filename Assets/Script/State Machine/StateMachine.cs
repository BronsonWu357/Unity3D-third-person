using System;
using UnityEngine;


//状态机，用来处理各种状态
public class StateMachine<T>
{
    //当前状态
    public State<T> CurrentState { get; private set; }

    public T owner;



    public StateMachine(T owner)
    {
        this.owner = owner;
    }


    public void ChangeState(State<T> newState)
    {
        //?可以避免抛出异常
        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter(owner);
    }


    //
    public void Execute()
    {
        CurrentState?.Execute();
    }

}
