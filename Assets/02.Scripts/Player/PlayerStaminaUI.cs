using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] PlayerStamina playerStamina;
    [SerializeField] Image staminaBar;

    private void Awake()
    {
        if(playerStamina == null)
            playerStamina = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStamina>();
    }
    private void OnEnable()
    {
        if (playerStamina != null)
            playerStamina.OnChangeStamina += UpdateSP;
    }

    private void OnDisable()
    {
        if (playerStamina != null)
            playerStamina.OnChangeStamina -= UpdateSP;
    }

    void UpdateSP(float maxValue, float currentValue)
    {
        staminaBar.fillAmount = currentValue / maxValue;
    }


}
