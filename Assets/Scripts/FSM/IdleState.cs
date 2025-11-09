using UnityEngine;
using UnityEngine.AI;

public class IdleState : MonoBehaviour , IState
{
    [SerializeField] private float idleTime = 3f;
    [SerializeField] private SoilderEnemyController enemyController;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private float timer = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<SoilderEnemyController>();
    }

    public void EnterState()
    {
        navMeshAgent.isStopped = true;
        timer = 0;
    }

    public void ExitState()
    {
        navMeshAgent.isStopped = false;
        timer = 0;
    }

    public void UpdateState()
    {
        // 플레이어가 추격할 수 있는 범위 안에 없는지 확인
        if (enemyController.IsInVisionRadius())
        {
            enemyController.UpdateState(EState.Chase);
            return;
        }

        if (enemyController.IsInAttackRadius())
        {
            enemyController.UpdateState(EState.Shoot);
            return;
        }

        timer += Time.deltaTime;
        if(timer >= idleTime)
        {
            enemyController.UpdateState(EState.Patrol);
            return;
        }
    }
}
