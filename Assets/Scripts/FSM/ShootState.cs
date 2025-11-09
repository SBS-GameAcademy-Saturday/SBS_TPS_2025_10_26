using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class ShootState : MonoBehaviour, IState
{
    [SerializeField] private Animator animator;
    [SerializeField] private SoilderEnemyController enemyController;

    [Header("Enemy Shot")]
    [SerializeField] private Transform shootingArea;
    [SerializeField] private float shootingRange = 90f;
    [SerializeField] private float normalRange = 0.03f;
    [SerializeField] private float timeShot = 1f;
    [SerializeField] private float shootDelay = 0.3f;

    [Header("슈팅 VFX")]
    [SerializeField] private ParticleSystem muzzleSpark;

    [Header("ShootState")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float shootingDamage = 10;

    private bool previousShoot = false;
    private float shootDelayTimer = 0;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<SoilderEnemyController>();
        animator = GetComponentInChildren<Animator>();
    }

    public void EnterState()
    {
        // NavMeshAgent의 이동을 막는 변수를 true 바꿔서
        // 공격중에 이동을 방지한다.
        navMeshAgent.isStopped = true;
        animator.SetBool("Shoot", true);
        shootDelayTimer = 0;
    }

    public void ExitState()
    {
        // NavMeshAgent의 이동을 막는 변수를 false 바꿔서
        // 이동이 가능하도록 바꿔준다.
        navMeshAgent.isStopped = false;
        animator.SetBool("Shoot", false);
        shootDelayTimer = 0;
    }

    public void UpdateState()
    {
        
        // 공격시에 항상 타겟을 바라봐야한다.
        transform.LookAt(target.position);

        if (!enemyController.IsInAttackRadius())
        {
            enemyController.UpdateState(EState.Chase);
            return;
        }

        if(shootDelayTimer <= shootDelay)
        {
            shootDelayTimer += Time.deltaTime;
            return;
        }

        // 이전에 총알을 발사한 경우
        if (previousShoot)
            return;
        previousShoot = true;

        muzzleSpark.Play();

        bool isHit = Physics.Raycast(shootingArea.transform.position, GetRandomForward(), 
            out RaycastHit hitInfo, shootingRange);
        if (isHit)
        {
            if(hitInfo.collider.TryGetComponent(out Damagable damagable))
            {
                damagable.HitDamage(shootingDamage);
            }
        }

        // timeShot 초가 지난 후에 "ActiveShooting"의 이름을 가진
        // 메서드를 호출한다.
        Invoke("ActiveShooting", timeShot);
    }

    // 총기 반동으로 인한 Spread 구현
    private Vector3 GetRandomForward()
    {
        Vector3 forward = shootingArea.transform.forward;

        forward.x = forward.x + Random.Range(-normalRange, normalRange);
        forward.y = forward.y + Random.Range(-normalRange, normalRange);

        return forward;
    }


    private void ActiveShooting()
    {
        previousShoot = false;
    }
}
