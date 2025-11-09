using UnityEngine;

public interface IState
{
    /// <summary>
    /// 현 상태에 들어올 때 호출하는 함수
    /// </summary>
    public void EnterState();
    /// <summary>
    /// 현 상태를 매 프레임마다 갱신하는 함수
    /// </summary>
    public void UpdateState();
    /// <summary>
    /// 현 상태를 빠져나갈 때 호출하는 함수
    /// </summary>
    public void ExitState();
}