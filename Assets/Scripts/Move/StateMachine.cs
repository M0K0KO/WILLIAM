using UnityEngine;
using UnityEngine.UIElements;

public class StateMachine
{
    private BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState.Update();
        Debug.Log(currentState.ToString());
    }

}