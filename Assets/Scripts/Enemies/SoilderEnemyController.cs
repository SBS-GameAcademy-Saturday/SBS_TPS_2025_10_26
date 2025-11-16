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

    // �þ�
    [Header("Vision")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float initialVisionRadius = 11f;
    [SerializeField] private float onHitVisionRadius = 30f;

    [Header("Attack")]
    [SerializeField] private float initialAttackRadius = 5f;
    [SerializeField] private float onHitAttackRadius = 15f;

    [Header("Animator")]
    [SerializeField] private Damagable damagable;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;


    [SerializeField] private EState currentState = EState.Patrol;

    private bool _isDeath = false;
    private bool _isHited = false;

    private float _visionRadius = 0;
    private float _attackRadius = 0;


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

        // �������� ���� Patrol ���·� ��ȯ
        UpdateState(currentState);
        damagable.OnDeath.AddListener(OnDeath);
        damagable.OnHealthChangedEvent.AddListener(OnDamaged);

        _visionRadius = initialVisionRadius;
        _attackRadius = initialAttackRadius;
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
        // ���� ���ϴ� ���·� ��ȯ
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
        // ���� ���� ����
        currentState = state;
    }

    public bool IsInVisionRadius()
    {
        return Physics.CheckSphere(transform.position, _visionRadius, playerLayer);
    }

    public bool IsInAttackRadius()
    {
        return Physics.CheckSphere(transform.position, _attackRadius, playerLayer);
    }

    private void OnDeath()
    {
        animator.SetTrigger("Death");
        navMeshAgent.isStopped = true;
        _isDeath = true;
        Destroy(gameObject, 2f);
    }

    private void OnDamaged(float currentHealth, float maxHealth)
    {
        if (_isHited)
            return;
        _isHited = true;
        _visionRadius = onHitVisionRadius;
        _attackRadius = onHitAttackRadius;
        UpdateState(EState.Shoot);
        Invoke("ResetOnHited",5);
    }

    private void ResetOnHited()
    {
        _isHited = false;
        _visionRadius = initialVisionRadius;
        _attackRadius = initialAttackRadius;
    }
}
