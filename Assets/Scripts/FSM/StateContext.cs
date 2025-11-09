using UnityEngine;

public enum EState
{
    Idle,
    Patrol,
    Chase,
    Shoot,
}


/// <summary>
/// 상태 전환을 책임지는 코드
/// </summary>
public class StateContext : MonoBehaviour
{
    private IState currentState;

    /// <summary>
    /// 현재 상태를 나타내는 프로퍼티
    /// </summary>
    public IState CurrentState 
    { 
        get 
        { 
            return currentState; 
        } 
    }

    /// <summary>
    /// 현재 상태를 새로운 상태로 전환하는 함수
    /// </summary>
    /// <param name="state"> 새로운 상태의 인터페이스</param>
    public void Transition(IState state)
    {
        // 현재 상태를 종료
        if(currentState != null) currentState.ExitState();
        // 새로운 상태로 변경
        currentState = state;
        // 새로운 상태로 진입
        currentState.EnterState();
    }
}
