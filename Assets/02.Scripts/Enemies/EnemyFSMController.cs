using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    NuckBack,
    Stun,
    Dead
}
public class EnemyFSMController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private EnemyHP enemyHP;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Transform targetPos;
    //공격 구현 안됨

    [Header("FSM")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;
    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float idleDistance = 3f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDistance = 1.5f;

    [SerializeField] LayerMask targetLayer;

    public event Action<EnemyState> OnEnemyStateChanged;

    public event Action OnAttack;
    public EnemyState CurrentState => currentState;
    public Vector2 CurrentDir {  get; private set; }
    public void SetCurrentState(EnemyState state) => TransitionTo(state);


    Coroutine attackCoroutine;
    private void Awake()
    {
        if(enemyHP == null) enemyHP = GetComponentInChildren<EnemyHP>();
        if(rb2D == null) rb2D = GetComponentInChildren<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnDied += HandleDead;
            enemyHP.OnHit += ApplyStun;
        }
    }
    private void OnDisable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnDied -= HandleDead;
            enemyHP.OnHit -= ApplyStun;
        }
    }
    private void TransitionTo(EnemyState nextState)
    {
        if (currentState == nextState) return;

        currentState = nextState;
        OnEnemyStateChanged?.Invoke(currentState);
    }
    private void FixedUpdate()
    {
        rb2D.linearVelocity = Vector2.zero;
        rb2D.angularVelocity = 0f;

        if (currentState == EnemyState.Chase)
        {
            HandleChaseMovement();
            return;
        }
        if (currentState == EnemyState.Idle)
        {
            CurrentDir = Vector2.zero;
            return;
        }
        if (currentState == EnemyState.Attack)
        {
            HandleAttack();
            return;
        }
    }

    private void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if(targetPos == null)
        {
            TryFindTarget();
            return;
        }

        EvaluateStateTransition();
    }
    Coroutine stunCoroutine = null;
    private void ApplyStun(float amount)
    {
        if(stunCoroutine == null)
        {
            stunCoroutine = StartCoroutine(SetStun());
        }
        else
        {
            StopCoroutine(stunCoroutine);
            stunCoroutine = StartCoroutine(SetStun());
        }
    }
    IEnumerator SetStun()
    {
        currentState = EnemyState.Stun;
        CurrentDir = Vector2.zero;
        float distanceToTarget = Vector2.Distance(transform.position, targetPos.position);

        yield return new WaitForSeconds(0.4f);

        if (distanceToTarget <= attackDistance)
        {
            TransitionTo(EnemyState.Attack);
        }
        else if (distanceToTarget > chaseDistance * 1.2f)
        {
            TransitionTo(EnemyState.Idle);
        }
        else
        {
            TransitionTo(EnemyState.Chase);
        }
    }

    private bool CanSeeTarget()
    {
        if(targetPos == null) return false;

        RaycastHit2D hit2D = Physics2D.Linecast(transform.position, targetPos.position, targetLayer);

        Debug.DrawLine(transform.position, hit2D.point, Color.red);
        if (hit2D.collider.gameObject.layer != targetPos.gameObject.layer)
        {
            return false;
        }
        return true;
    }
    private void EvaluateStateTransition()
    {
        if (targetPos == null) return;
        

        float distanceToTarget = Vector2.Distance(transform.position, targetPos.position);
        if (!CanSeeTarget())
        {
            TransitionTo(EnemyState.Idle);
            return;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToTarget <= chaseDistance)
                {
                    TransitionTo(EnemyState.Chase);
                }
                break;
            case EnemyState.Chase:
                if (distanceToTarget <= attackDistance)
                {
                    TransitionTo(EnemyState.Attack);
                }
                else if (distanceToTarget > chaseDistance * 1.2f)
                {
                    TransitionTo(EnemyState.Idle);
                }
                break;
            case EnemyState.Attack:
                if (distanceToTarget > attackDistance)
                {
                    TransitionTo(EnemyState.Chase);
                }
                break;
        }
    }
    private void TryFindTarget()
    {
        if (targetPos != null) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            targetPos = playerObject.transform;
        }
    }

    private void HandleChaseMovement()
    {
        rb2D.AddForce(Vector2.zero);
        if(targetPos == null || rb2D == null) return;
        if (currentState != EnemyState.Chase) return;

        CurrentDir = (Vector2)(targetPos.position - transform.position).normalized;
        
        rb2D.MovePosition(rb2D.position + CurrentDir * moveSpeed * Time.fixedDeltaTime);
    }
    private void HandleAttack()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }

    }
    IEnumerator Attack()
    {
        CurrentDir = Vector2.zero;
        IDamageable damageable = targetPos.GetComponentInChildren<IDamageable>();

        if (damageable == null) yield break;

        if (enemyHP.IsDead) yield break;
        enemyAttack.OnAttack(damageable);
        OnAttack?.Invoke();
        yield return new WaitForSeconds(enemyAttack.AttackCoolDown);
        attackCoroutine = null;
    }


    private void HandleDead()
    {
        if(attackCoroutine != null) StopCoroutine (attackCoroutine);
        TransitionTo(EnemyState.Dead);
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.brown;
        Gizmos.DrawWireSphere(transform.position,idleDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance * 1.2f);
        Gizmos.color = Color.black;
        if (targetPos != null) Gizmos.DrawLine(transform.position, targetPos.position);
    }
}
