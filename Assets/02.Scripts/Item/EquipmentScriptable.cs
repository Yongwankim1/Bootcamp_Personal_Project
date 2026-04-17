using UnityEngine;

[CreateAssetMenu(fileName = "EquipItem", menuName = "Item/EquipmentItem")]
public class EquipmentScriptable : ItemScriptable
{
    public override void Use(Inventory inventory)
    {
        if (PlayerBaseEquipment.Instance == null) return;
        Debug.Log(ItemData.ItemID);
        PlayerBaseEquipment.Instance.SetArmor(ItemData.ItemID, out string backItemID);
        inventory.AddItem(backItemID, 1);
    }
}
