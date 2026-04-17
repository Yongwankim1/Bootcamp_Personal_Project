using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] PlayerStamina playerStamina;
    [SerializeField] ItemScriptable weapon;

    [SerializeField] private bool isAttack;

    [SerializeField] private float coolTime = -999f;
    [SerializeField] private float nextCoolTime = -999f;

    [SerializeField] float attackDistance;
    [SerializeField] LayerMask targetLayer;
    private void Update()
    {
        if (inputReader == null) return;
        isAttack = inputReader.IsAttackPerformedThisFrame;
        coolTime = Time.time;

        OnAttack();
        
    }
    void OnAttack()
    {
        if (!isAttack) return;
        if (coolTime < nextCoolTime) return;
        if (weapon == null) return;
        if (weapon.ItemData.AttackType == AttackType.None) return;
        if (!playerStamina.IsAttack) return;
        switch (weapon.ItemData.AttackType)
        {
            case AttackType.Melee: MeleeAttack(); break;
            case AttackType.Range: RangedAttack(); break;
        }
        nextCoolTime = Time.time + weapon.ItemData.AttackCoolDown;
        playerStamina.AttackDecraseValue();
    }

    void MeleeAttack()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;
        if (weapon is IAttack attack)
        {
            attack.Attack(transform, attackDistance, mouseWorldPos,targetLayer);
        }
    }
    void RangedAttack()
    {

    }
}
