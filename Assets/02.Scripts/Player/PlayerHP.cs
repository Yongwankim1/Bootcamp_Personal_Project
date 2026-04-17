using System;
using UnityEngine;

public class PlayerHP : CharacterHP
{
    [SerializeField] float equipHP;
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public event Action<float,float> OnChangeHP;
    private void Awake()
    {
        Initialize();
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        OnChangeHP?.Invoke(maxHP,currentHP);
    }
    private void Initialize()
    {
        if (maxHP <= 0) maxHP = 1;
        if (PlayerBaseState.Instacne != null)
        {
            maxHP = PlayerBaseState.Instacne.MaxHP;
            currentHP = PlayerBaseState.Instacne.CurrentHP;
        }
        currentHP = Mathf.Clamp(currentHP, 1, maxHP);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        OnChangeHP?.Invoke(maxHP, currentHP);
    }
    private void OnDisable()
    {
        if (PlayerBaseState.Instacne == null) return;

        PlayerBaseState.Instacne.MaxHP = maxHP;
        PlayerBaseState.Instacne.CurrentHP = currentHP;
    }
    protected override void Died()
    {
        base.Died();
    }
}
