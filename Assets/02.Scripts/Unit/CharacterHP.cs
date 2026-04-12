using System;
using UnityEngine;

public class CharacterHP : MonoBehaviour, IDamageable
{
    [SerializeField] protected float currentHP;
    [SerializeField] protected float maxHP;
    [SerializeField] Animator animator;
    [SerializeField] string onDieParameterName = "OnDie";
    private int dieHash;
    public bool IsDead => currentHP <= 0;
    public event Action OnDied;
    public event Action OnHit;
    void Awake()
    {
        if(animator == null)
            animator = GetComponent<Animator>();

    }
    void Start()
    {
        dieHash = Animator.StringToHash(onDieParameterName);
    }
    public void TakeDamage(float amount)
    {
        if(IsDead || amount <= 0) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        if (IsDead)
        {
            Died();
            return;
        }
        OnHit?.Invoke();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;

        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
    }

    protected virtual void Died()
    {
        animator.SetTrigger(dieHash);
        OnDied?.Invoke();
        
        //TODO:Á×¾úÀ» ¶§

    }


}
