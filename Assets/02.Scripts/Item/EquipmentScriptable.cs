using UnityEngine;

[CreateAssetMenu(fileName = "EquipItem", menuName = "Item/EquipmentItem")]
public class EquipmentScriptable : ItemScriptable
{
    public override void Use(Inventory inventory)
    {
        if (PlayerBaseEquipment.Instance == null) return;
        Debug.Log(ItemData.ItemID);
        if(ItemData.Type == ItemType.BackPack)
        {
            if (!PlayerInventoryData.Instance.CheckChangeItems(ItemData.Value1)) return;
        }
        PlayerBaseEquipment.Instance.Equip(ItemData.ItemID, out string backItemID);
        inventory.AddItem(backItemID, 1);
    }
}
