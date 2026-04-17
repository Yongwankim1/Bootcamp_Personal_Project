using System;
using UnityEngine;
[DefaultExecutionOrder(-997)]
public class PlayerInventoryData : MonoBehaviour
{
    public static PlayerInventoryData Instance;

    [SerializeField] ItemPositiones[] baseBackPack;

    public ItemPositiones[] BaseBackPack => baseBackPack;

    public event Action OnChangeBackpackData;
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
    }
    private void Start()
    {
        SetBackPack();
    }
    private void OnEnable()
    {
        if(PlayerBaseEquipment.Instance != null)
        {
            PlayerBaseEquipment.Instance.OnChangeBackpack += SetBackPack;
        }
    }

    private void OnDisable()
    {
        if (PlayerBaseEquipment.Instance != null)
        {
            PlayerBaseEquipment.Instance.OnChangeBackpack -= SetBackPack;
        }
    }
    public void SetBackPack()
    {
        if (!ItemCatalogManager.Instance.TryGetItemData(PlayerBaseEquipment.Instance.BackPackID, out var itemdata))
        {
            Debug.Log("아이템 없음");
            return;
        }
        ItemPositiones[] items = baseBackPack;
        baseBackPack = new ItemPositiones[itemdata.Value1];

        for (int i = 0; i < items.Length; i++)
        {
            baseBackPack[i] = items[i];
        }
        OnChangeBackpackData?.Invoke();
    }
}
