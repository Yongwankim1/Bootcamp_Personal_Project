using UnityEngine;
using System.Collections.Generic;
using System;
[System.Serializable]
public struct ItemPositiones
{
    public string ItemID;
    public int Amount;
    public int MaxAmount;
}
public class Inventory : MonoBehaviour
{
    [SerializeField] string backpackID;
    [SerializeField] ItemPositiones[] itemPositiones;
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] InventoryGUI inventoryGUI;
    public ItemPositiones[] ItemPositiones => itemPositiones;

    Dictionary<string, int> itemIdByCount = new Dictionary<string, int>();
    public Dictionary<string,int> ItemIdByCount => itemIdByCount;

    public event Action OnChangeItem;
    public event Action OnBackPackChanage;
    private void RaiseOnChanageItem() => OnChangeItem?.Invoke();
    private void RaiseOnBackPackChange() => OnBackPackChanage?.Invoke();
    private void Awake()
    {
        if(inputReader == null) inputReader.GetComponent<PlayerInputReader>();
        if (inventoryGUI == null) inventoryGUI = GameObject.Find("InventoryPanel").GetComponent<InventoryGUI>();
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (inputReader == null) return;
        if(inputReader.IsInventoryPerformedThisFrame)
        {
            inventoryGUI.gameObject.SetActive(!inventoryGUI.gameObject.activeSelf);
        }
    }
    void Init()
    {
        itemPositiones = new ItemPositiones[8];
        RaiseOnBackPackChange();
    }
    public void Init(string backpackID)
    {
        if (!ItemCatalogManager.Instance.TryGetItemData(backpackID, out ItemData itemData))
        {
            itemPositiones = new ItemPositiones[8];
            return;
        }
        this.backpackID = backpackID;
        itemPositiones = new ItemPositiones[itemData.Value1];
        RaiseOnBackPackChange();
    }
    public int AddItem(string itemId, int amount)
    {
        return IncreaseItem(itemId, amount);
    }

    private int IncreaseItem(string itemId, int amount)
    {
        if (amount <= 0) return 0;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemId, out var data)) return amount;

        int restAmount = amount;
        for (int i = 0; i < itemPositiones.Length; i++)
        {
            if (string.IsNullOrEmpty(itemPositiones[i].ItemID)) continue;
            if (itemPositiones[i].Amount >= itemPositiones[i].MaxAmount) continue;

            if (itemPositiones[i].ItemID == itemId && itemPositiones[i].Amount < itemPositiones[i].MaxAmount)
            {
                int canAdd = itemPositiones[i].MaxAmount - itemPositiones[i].Amount;
                int addAmount = Mathf.Min(canAdd, restAmount);

                itemPositiones[i].Amount += addAmount;
                restAmount -= addAmount;

                if (itemIdByCount.ContainsKey(itemId))
                {
                    itemIdByCount[itemId] += addAmount;
                }
                else
                {
                    itemIdByCount.Add(itemId, addAmount);
                }
                if (restAmount <= 0)
                {
                    RaiseOnChanageItem();
                    return restAmount;
                }
            }
        }
        for (int i = 0; i < itemPositiones.Length; i++)
        {
            if (!string.IsNullOrEmpty(itemPositiones[i].ItemID)) continue;
            itemPositiones[i].ItemID = itemId;
            itemPositiones[i].MaxAmount = data.MaxStack;
            int canAdd = itemPositiones[i].MaxAmount + itemPositiones[i].Amount;
            int addAmount = Math.Min(canAdd, restAmount);

            itemPositiones[i].Amount = addAmount;
            restAmount -= addAmount;
            if (itemIdByCount.ContainsKey(itemId))
            {
                itemIdByCount[itemId] += addAmount;
            }
            else
            {
                itemIdByCount.Add(itemId, addAmount);
            }
            if (restAmount <= 0)
            {
                RaiseOnChanageItem();
                return restAmount;
            }
        }


        RaiseOnChanageItem();
        return restAmount;
    }

    private void DecreaseItem(string itemId, int amount)
    {
        if (amount <= 0) return;

        if(itemIdByCount.ContainsKey(itemId))
        {
            itemIdByCount[itemId] -= amount;
            if (itemIdByCount[itemId] <= 0)
            {
                itemIdByCount.Remove(itemId);
            }
        }
        RaiseOnChanageItem();
    }

    public void OnInventory()
    {
        if (inventoryGUI == null) return;

        inventoryGUI.gameObject.SetActive(!inventoryGUI.gameObject.activeSelf);

    }
}
