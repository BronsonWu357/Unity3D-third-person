using System;
using UnityEngine;


//״̬���������������״̬
public class StateMachine<T>
{
    //��ǰ״̬
    public State<T> CurrentState { get; private set; }

    public T owner;



    public StateMachine(T owner)
    {
        this.owner = owner;
    }


    public void ChangeState(State<T> newState)
    {
        //?���Ա����׳��쳣
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
