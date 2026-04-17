using UnityEngine;

public class PlayerBaseState : MonoBehaviour
{
    public static PlayerBaseState Instacne;
    public float MaxHP;
    public float CurrentHP;
    public float StaminaPoint;
    public float CurrentStamina;
    public float AttackDamage;

    private void Awake()
    {
        if(Instacne == null)
        {
            Instacne = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}


