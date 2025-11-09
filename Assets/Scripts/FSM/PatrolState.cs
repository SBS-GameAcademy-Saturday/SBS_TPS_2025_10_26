using UnityEngine;
using UnityEngine.AI;

public class PatrolState : MonoBehaviour, IState
{
    [SerializeField] private SoilderEnemyController enemyController;
    [SerializeField] private NavMeshAgent navMeshAgent;
    // 이동해야할 웨이포이트 위치들
    [SerializeField] private Transform[] waypoints;
    // 도착 여부를 판단할 변수
    [SerializeField] private float arriveDistance = 0.3f;


    private int currentWayPointIndex = 0;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<SoilderEnemyController>();
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        // 플레이어가 추격할 수 있는 범위 안에 들어왔는 지 확인
        if(enemyController.IsInVisionRadius())
        {
            enemyController.UpdateState(EState.Chase);
            return;
        }


        // 나의 위치와 현재 목적지와의 거리값을 구한다.
        float distance = Vector3.Distance(waypoints[currentWayPointIndex].position,
            transform.position);
       
        // 목적지와의 거리를 비교해서 도착한 상태이면 다음 목적지로 이동한다.
        if (distance <= arriveDistance)
        {
            currentWayPointIndex++;
            if(currentWayPointIndex >= waypoints.Length)
            {
                currentWayPointIndex = 0;
            }
            enemyController.UpdateState(EState.Idle);
            return;
        }
        // NavMeshAgent에 목적지 갱신
        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
    }
}