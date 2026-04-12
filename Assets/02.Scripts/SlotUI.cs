using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct SlotData
{
    public string ItemID;
    public int Amount;
}

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] InventoryGUI inventoryGUI;

    public SlotData SlotData;
    
    public void Initialize()
    {
        SlotData = new SlotData();
        iconImage.gameObject.SetActive(false);
    }
    public void Initialize(string itemID, int amount, InventoryGUI inventoryGUI)
    {
        this.inventoryGUI = inventoryGUI;
        Initialize();
        if (!ItemCatalogManager.Instance.TryGetItemData(itemID, out ItemData itemdata))
        {
            return;
        }
        SlotData.ItemID = itemID;
        SlotData.Amount = amount;
        if(SlotData.Amount <= 1)
        {
            amountText.text = string.Empty;
        }
        iconImage.sprite = itemdata.ItemIcon;
        iconImage.gameObject.SetActive(true);
    }

    public void Initialize(SlotData slotData)
    {
        SlotData = slotData;
    }
}
