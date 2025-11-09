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
        // �÷��̾ �߰��� �� �ִ� ���� �ȿ� ������ Ȯ��
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
