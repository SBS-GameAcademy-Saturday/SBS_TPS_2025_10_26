using UnityEngine;
using UnityEngine.AI;

public class ChaseState : MonoBehaviour, IState
{
    [SerializeField] private SoilderEnemyController enemyController;
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float chaseSpeed = 3.5f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<SoilderEnemyController>();
    }

    public void EnterState()
    {
        navMeshAgent.speed = chaseSpeed;
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        // 플레이어가 추격할 수 있는 범위 안에 없는지 확인
        if (!enemyController.IsInVisionRadius())
        {
            enemyController.UpdateState(EState.Patrol);
            return;
        }

        if(enemyController.IsInAttackRadius())
        {
            enemyController.UpdateState(EState.Shoot);
            return;
        }


        navMeshAgent.SetDestination(target.position);
    }
}
