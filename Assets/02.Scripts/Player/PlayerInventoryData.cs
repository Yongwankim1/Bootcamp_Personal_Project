using System;
using System.Collections.Generic;
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
    public int CheckChangeItems()
    {
        int count = 0;

        for(int i = 0; i < baseBackPack.Length; i++)
        {
            if (string.IsNullOrEmpty(baseBackPack[i].ItemID)) continue;

            count++;
        }

        return count;
    }
    public bool CheckChangeItems(int amount)
    {
        int count = 0;

        for (int i = 0; i < baseBackPack.Length; i++)
        {
            if (string.IsNullOrEmpty(baseBackPack[i].ItemID)) continue;

            count++;
        }

        if(count >= amount)
        {
            return false;
        }
        return true;
    }

    public void SetBackPack()
    {
        int amount = 8;
        if (ItemCatalogManager.Instance.TryGetItemData(PlayerBaseEquipment.Instance.BackPackID, out var itemdata))
        {
            amount = itemdata.Value1;
        }
        if (CheckChangeItems() > amount) return;

        ItemPositiones[] items = baseBackPack;
        baseBackPack = new ItemPositiones[amount];
        List<ItemPositiones> itemlist = new List<ItemPositiones>();
        for (int i = 0; i < items.Length; i++)
        {
            if (string.IsNullOrEmpty(items[i].ItemID)) continue;

            itemlist.Add(items[i]);
        }
        for(int i = 0; i < itemlist.Count; i++)
        {
            baseBackPack[i] = itemlist[i];
        }
        OnChangeBackpackData?.Invoke();
    }


}
