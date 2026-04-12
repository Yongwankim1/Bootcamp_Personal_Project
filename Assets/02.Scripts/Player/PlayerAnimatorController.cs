using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] Animator targetAnimator;
    [SerializeField] PlayerInputReader inputReader;

    [SerializeField] string moveXParameterName;
    [SerializeField] string moveYParameterName;
    [SerializeField] string isMoveParameterName;

    private int moveXHash;
    private int moveYHash;
    private int isMoveHash;

    float moveDeadZone = 0.01f;

    Vector2 currentMoveDir = Vector2.zero;
    bool isMove = false;
    private void Awake()
    {
        if(targetAnimator == null) targetAnimator = GetComponent<Animator>();
        if(inputReader == null) inputReader = GetComponentInParent<PlayerInputReader>();

        moveXHash = Animator.StringToHash(moveXParameterName);
        moveYHash = Animator.StringToHash(moveYParameterName);
        isMoveHash = Animator.StringToHash(isMoveParameterName);
    }

    private void Update()
    {
        if (targetAnimator == null) return;

        if(inputReader != null)
        {
            currentMoveDir = inputReader.MoveDir;
        }
        isMove = currentMoveDir.sqrMagnitude > moveDeadZone;

        SetParameter(currentMoveDir, isMove);
    }

    private void SetParameter(Vector2 dir, bool isMove)
    {
        targetAnimator.SetFloat(moveXHash, currentMoveDir.x);
        targetAnimator.SetFloat(moveYHash, currentMoveDir.y);
        targetAnimator.SetBool(isMoveHash, isMove);
    }
}
