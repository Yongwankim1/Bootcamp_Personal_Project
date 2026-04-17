using UnityEngine;

public class PlayerInventoryData : MonoBehaviour
{
    public static PlayerInventoryData Instance;

    [SerializeField] ItemPositiones[] baseBackPack;

    public ItemPositiones[] BaseBackPack => baseBackPack;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        baseBackPack = new ItemPositiones[8];
    }

    public void SetBackPack(ItemPositiones[] baseBackPack)
    {
        this.baseBackPack = baseBackPack;
    }
}
