using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField] EnemyFSMController fSMController;
    [SerializeField] Animator targetAnimator;

    [SerializeField] EnemyHP enemyHP;

    [SerializeField] string moveXParameterName;
    [SerializeField] string moveYParameterName;
    [SerializeField] string isMoveParameterName;
    [SerializeField] string onHitParameterName;
    [SerializeField] string onDieParameterName;
    [SerializeField] string onAttackParameterName = "OnAttack";

    private int moveXHash;
    private int moveYHash;
    private int isMoveHash;
    private int onHitHash;
    private int onDieHash;
    private int onAttackHash;

    float moveDeadZone = 0.01f;

    Vector2 currentMoveDir = Vector2.zero;
    bool isMove = false;
    private void Awake()
    {
        if (targetAnimator == null) targetAnimator = GetComponent<Animator>();
        if (enemyHP == null) enemyHP = GetComponent<EnemyHP>();
        if(fSMController == null) fSMController = GetComponentInParent<EnemyFSMController>();
        moveXHash = Animator.StringToHash(moveXParameterName);
        moveYHash = Animator.StringToHash(moveYParameterName);
        isMoveHash = Animator.StringToHash(isMoveParameterName);
        onHitHash = Animator.StringToHash(onHitParameterName);
        onDieHash = Animator.StringToHash(onDieParameterName);
        onAttackHash = Animator.StringToHash(onAttackParameterName);
    }
    private void OnEnable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnHit += OnHitTrigger;
            enemyHP.OnDied += OnDieTrigger;
        }
        if(fSMController != null)
        {
            fSMController.OnAttack += OnAttackTrigger;
        }
    }
    private void OnDisable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnHit -= OnHitTrigger;
            enemyHP.OnDied -= OnDieTrigger;
        }
        if (fSMController != null)
        {
            fSMController.OnAttack -= OnAttackTrigger;
        }
    }
    private void Update()
    {
        if (targetAnimator == null && fSMController == null) return;
        currentMoveDir = fSMController.CurrentDir;
        isMove = currentMoveDir.sqrMagnitude > moveDeadZone;

        SetParameter(currentMoveDir, isMove);
    }

    private void SetParameter(Vector2 dir, bool isMove)
    {
        targetAnimator.SetFloat(moveXHash, currentMoveDir.x);
        targetAnimator.SetFloat(moveYHash, currentMoveDir.y);
        targetAnimator.SetBool(isMoveHash, isMove);
    }
    private void OnAttackTrigger()
    {
        targetAnimator.SetTrigger(onAttackHash);
    }
    private void OnDieTrigger()
    {
        targetAnimator.SetTrigger(onDieHash);
    }
    private void OnHitTrigger(float amount)
    {
        targetAnimator.SetTrigger(onHitHash);
    }
}
