using UnityEngine;

public class PlayerBaseState : MonoBehaviour
{
    public static PlayerBaseState Instacne;
    [Header("HP, Stamina State")]
    public float MaxHP;
    public float CurrentHP;
    public float MaxStamina;
    
    [Header("SurivalState")]
    public float Hydration;
    public float MaxHydration;
    public float hunger;
    public float MaxHunger;

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


