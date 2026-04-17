using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float damge = 5f;
    [SerializeField] float attackCoolDown = 1f;
     public float AttackCoolDown => attackCoolDown;

    public void OnAttack(IDamageable damageable)
    {
        Debug.Log("АјАн");
        damageable.TakeDamage(damge);
    }

}
