using UnityEngine;

public class PlayerStashManager : MonoBehaviour
{
    public static PlayerStashManager Instance;
    
    [SerializeField] int slotMaxIndex;

    public ItemPositiones[] slotUIs = new ItemPositiones[100];

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        slotUIs = new ItemPositiones[slotMaxIndex];
    }



}
