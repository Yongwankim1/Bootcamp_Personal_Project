using System;
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;

    [Header("Value")]
    [SerializeField] float maxStamina;
    [SerializeField] float currentStamina;
    [SerializeField] float walkDeCreaseValue;
    [SerializeField] float runDeCraseValue;
    [SerializeField] float inCreaseValue;
    [SerializeField] float attactDeCreaseValue;
    [Header("Option")]
    [SerializeField] bool isDebug;

    public float MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;

    public event Action<float,float> OnChangeStamina;

    public bool IsAttack => CurrentStamina >= attactDeCreaseValue && !IsStaminaDepleted;

    public bool IsStaminaDepleted {  get; private set; }

    Coroutine staminaIncrease;
    private void Awake()
    {
        if(PlayerBaseState.Instacne != null)
        {
            maxStamina = PlayerBaseState.Instacne.StaminaPoint;
            currentStamina = MaxStamina;
        }
        else
        {
            if (maxStamina <= 0) maxStamina = 100f;
            currentStamina = maxStamina;
        }
        if(inputReader == null) inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        SetWalkStamina(walkDeCreaseValue);

        SetRunStamina(runDeCraseValue);
    }

    private void SetWalkStamina(float walkValue)
    {
        if (IsStaminaDepleted) return;
        if (inputReader.MoveAction())
        {
            DeCreaseCurrentStamina(walkValue);
        }
        else
        {
            InCreaseCurrentStamina();
        }
    }




    private void SetRunStamina(float runValue)
    {
        if (IsStaminaDepleted) return;
        if (inputReader.RunAction())
        {
            DeCreaseCurrentStamina(runValue);
        }
    }
    public void AttackDecraseValue()
    {
        if (IsStaminaDepleted) return;
        currentStamina -= attactDeCreaseValue;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        OnChangeStamina?.Invoke(maxStamina, currentStamina);
        if (isDebug)
            Debug.Log($"스테미나 감소량 {attactDeCreaseValue} 남은 스테미나{currentStamina} 최대 스테미나 {maxStamina}");
        if (staminaIncrease == null)
        {
            staminaIncrease = StartCoroutine(IncreaseCurrentStamina(inCreaseValue));
        }
        else
        {
            StopCoroutine(staminaIncrease);
            staminaIncrease = StartCoroutine(IncreaseCurrentStamina(inCreaseValue));
        }

    }
    private void InCreaseCurrentStamina()
    {
        if (staminaIncrease == null)
        {
            staminaIncrease = StartCoroutine(IncreaseCurrentStamina(inCreaseValue));
        }
    }
    private void DeCreaseCurrentStamina(float amount)
    {
        if (staminaIncrease != null)
        {
            StopCoroutine(staminaIncrease);
            staminaIncrease = null;
        }
        currentStamina -= amount * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0 , maxStamina);

        OnChangeStamina?.Invoke(maxStamina, currentStamina);

        if (isDebug)
            Debug.Log($"스테미나 감소량 {amount} 남은 스테미나{currentStamina} 최대 스테미나 {maxStamina}");

        if(currentStamina <= 0)
        {
            IsStaminaDepleted = true;
            if (staminaIncrease == null)
            {
                staminaIncrease = StartCoroutine(RecoverStamina());
            }
            else
            {
                StopCoroutine(staminaIncrease);
                staminaIncrease = StartCoroutine(RecoverStamina());
            }
        }
    }
    IEnumerator RecoverStamina()
    {
        while (true)
        {
            currentStamina += inCreaseValue * 1.3f * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            OnChangeStamina?.Invoke(maxStamina, currentStamina);
            if (isDebug)
                Debug.Log($"스테미나 증가량 {inCreaseValue} 남은 스테미나{currentStamina} 최대 스테미나 {maxStamina}");
            if(currentStamina >= maxStamina / 2)
            {
                IsStaminaDepleted = false;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator IncreaseCurrentStamina(float amount)
    {
        yield return new WaitForSeconds(1.5f);
        while (true)
        {
            currentStamina += amount * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);

            OnChangeStamina?.Invoke(maxStamina, currentStamina);

            if (isDebug)
                Debug.Log($"스테미나 증가량 {amount} 남은 스테미나{currentStamina} 최대 스테미나 {maxStamina}");
            yield return null;
        }
    }
}
