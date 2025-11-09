using JetBrains.Annotations;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SoilderEnemyController : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private StateContext stateContext;
    [SerializeField] private IdleState idleState;
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ShootState shootState;
    [SerializeField] private ChaseState chaseState;

    // 시야
    [Header("Vision")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float visionRadius = 11f;

    [Header("Attack")]
    [SerializeField] private float attackRadius = 5f;

    [Header("Animator")]
    [SerializeField] private Damagable damagable;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;


    [SerializeField] private EState currentState = EState.Patrol;

    private bool _isDeath = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        damagable = GetComponent<Damagable>();

        stateContext = GetComponent<StateContext>();
        idleState = GetComponent<IdleState>();
        patrolState = GetComponent<PatrolState>();
        shootState = GetComponent<ShootState>();
        chaseState = GetComponent<ChaseState>();

        // 시작하자 마다 Patrol 상태로 전환
        UpdateState(currentState);
        damagable.OnDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDeath)
            return;
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        stateContext.CurrentState.UpdateState();
    }

    public void UpdateState(EState state)
    {
        // 내가 원하는 상태로 전환
        switch(state)
        {
            case EState.Idle:
                stateContext.Transition(idleState);
                break;
            case EState.Patrol:
                stateContext.Transition(patrolState);
                break;
            case EState.Chase:
                stateContext.Transition(chaseState);
                break;
            case EState.Shoot:
                stateContext.Transition(shootState);
                break;
        }
        // 현재 상태 갱신
        currentState = state;
    }

    public bool IsInVisionRadius()
    {
        return Physics.CheckSphere(transform.position, visionRadius, playerLayer);
    }

    public bool IsInAttackRadius()
    {
        return Physics.CheckSphere(transform.position, attackRadius, playerLayer);
    }

    private void OnDeath()
    {
        animator.SetTrigger("Death");
        _isDeath = true;
        Destroy(gameObject, 2f);
    }

}
