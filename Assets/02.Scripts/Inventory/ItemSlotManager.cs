using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSlotManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EnemyInventory enemyInventory;
    [SerializeField] ItemDetailPanel itemPanel;
    [SerializeField] PlayerStashUI playerStashUI;

    public SlotType DragType;
    public SlotType DropType;

    public SlotData SelectDrag;
    public SlotData SelectDrop;

    public void Initialize(EnemyInventory enemyInventory)
    {
        this.enemyInventory = enemyInventory;
    }
    private void OnDisable()
    {
        enemyInventory = null;
    }
    public bool InventoryUseItem(string itemId, int index, int amount) => inventory.UseItem(itemId,index,amount);
    void Initialize()
    {
        DragType = SlotType.None;
        DropType = SlotType.None;

        SelectDrag = default;
        SelectDrop = default;
    }
    public void OnDetailPanel(string itemID,int index, SlotType type)
    {
        itemPanel.Initialize(itemID, index,type);
    }
    public void QuickSlotChange(int clickIndex, string itemId, int amount, SlotType type)
    {

        switch (type)
        {
            case SlotType.None: break;
            case SlotType.Enemy:
                if (enemyInventory == null) return;
                if (inventory.RemainingBagCount() > 0)
                {
                    inventory.AddItem(itemId, amount);
                    enemyInventory.ListRemove(clickIndex);
                }
                break;
            case SlotType.Player:

                if (enemyInventory == null && playerStashUI == null)
                {
                    Debug.Log("리턴됨");
                    return;
                }
                if (!inventory.RemoveItem(itemId, clickIndex, amount))
                {
                    Initialize();
                    return;
                }
                if(enemyInventory == null && playerStashUI)
                {
                    playerStashUI.AddItemSlot(itemId, amount);
                }
                else
                    enemyInventory.ListAddItem(itemId, amount);
                break;
            case SlotType.Stash:
                if (inventory.RemainingBagCount() > 0)
                {
                    playerStashUI.RemoveItemSlot(clickIndex);
                    inventory.AddItem(itemId, amount);
                }
                break;
            case SlotType.Equip:
                if(inventory.RemainingBagCount() > 0)
                {
                    inventory.AddItem(itemId,amount);
                    PlayerBaseEquipment.Instance.UnEquip(clickIndex);
                }
                break;
        }
        Initialize();
    }
    public bool InventorySlotChange()
    {
        if (DragType == SlotType.Player && DropType == SlotType.Player)
        {
            inventory.PositionChange(SelectDrag.SlotIndex, SelectDrop.SlotIndex);
            SelectDrag = default;
            SelectDrop = default;
        }
        else if (DragType == SlotType.Enemy && DropType == SlotType.Player)
        {
            if (inventory.AddItem(SelectDrag.ItemID, SelectDrag.Amount) >= 1)
            {
                Initialize();
                return false;
            }
            Debug.Log("추가됨");
            enemyInventory.ListRemove(SelectDrag.SlotIndex);
        }
        else if (DragType == SlotType.Player && DropType == SlotType.Enemy)
        {
            if (!inventory.RemoveItem(SelectDrag.ItemID, SelectDrag.SlotIndex, SelectDrag.Amount))
            {
                Initialize();
                return false;
            }
            enemyInventory.ListAddItem(SelectDrag.ItemID, SelectDrag.Amount);
            inventory.AddItem(SelectDrop.ItemID, SelectDrop.Amount);
        }
        else if (DragType == SlotType.Player && DropType == SlotType.Stash)
        {
            if (!inventory.RemoveItem(SelectDrag.ItemID, SelectDrag.SlotIndex, SelectDrag.Amount))
            {
                Debug.Log("리턴됨");
                Initialize();
                return false;
            }
            playerStashUI.DrawSlot(SelectDrag.ItemID, SelectDrag.Amount, SelectDrop.SlotIndex);
            inventory.AddItem(SelectDrop.ItemID, SelectDrop.Amount);

        }
        else if (DragType == SlotType.Stash && DropType == SlotType.Player)
        {
            playerStashUI.RemoveItemSlot(SelectDrag.SlotIndex);
            inventory.AddItem(SelectDrag.ItemID, SelectDrag.Amount, SelectDrop.SlotIndex);
            playerStashUI.DrawSlot(SelectDrop.ItemID, SelectDrop.Amount, SelectDrag.SlotIndex);

        }
        else if (DragType == SlotType.Stash && DropType == SlotType.Stash)
        {
            playerStashUI.SlotChange(SelectDrag.SlotIndex, SelectDrop.SlotIndex);
        }
        else if (DragType == SlotType.Player && DropType == SlotType.Equip)
        {
            if (!IsVailedCheck(SelectDrag.ItemID, SelectDrop.ItemID)) return false;

            inventory.RemoveItem(SelectDrag.ItemID, SelectDrag.SlotIndex, SelectDrag.Amount);
            PlayerBaseEquipment.Instance.Equip(SelectDrag.ItemID, out string backItemID);
            inventory.AddItem(backItemID, 1);
        }
        else if (DragType == SlotType.Equip && DropType == SlotType.Player)
        {
            if (!IsVailedCheck(SelectDrag.ItemID, SelectDrop.ItemID)) return false;
            PlayerBaseEquipment.Instance.UnEquip(SelectDrag.SlotIndex);
            inventory.RemoveItem(SelectDrop.ItemID, SelectDrop.SlotIndex, SelectDrop.Amount);
            PlayerBaseEquipment.Instance.Equip(SelectDrop.ItemID, out _);
            inventory.AddItem(SelectDrag.ItemID, 1, SelectDrop.SlotIndex);
        }
        else if (DragType == SlotType.Stash && DropType == SlotType.Equip)
        {
            if (!IsVailedCheck(SelectDrag.ItemID, SelectDrop.ItemID)) return false;
            playerStashUI.RemoveItemSlot(SelectDrag.SlotIndex);
            PlayerBaseEquipment.Instance.Equip(SelectDrag.ItemID, out string backItemID);
            playerStashUI.DrawSlot(backItemID, 1, SelectDrag.SlotIndex);
        }
        else if (DragType == SlotType.Equip && DropType == SlotType.Stash)
        {
            if (!IsVailedCheck(SelectDrag.ItemID, SelectDrop.ItemID)) return false;
            //아이템 확인 먼저 필요
            PlayerBaseEquipment.Instance.UnEquip(SelectDrag.SlotIndex);
            playerStashUI.RemoveItemSlot(SelectDrop.SlotIndex);
            playerStashUI.DrawSlot(SelectDrag.ItemID,1, SelectDrop.SlotIndex);
            PlayerBaseEquipment.Instance.Equip(SelectDrop.ItemID, out _);
        }
        Initialize();
        return true;
    }

    private bool IsVailedCheck(string dragID, string dropID)
    {
        ItemType dragType = ItemType.None;
        ItemType dropType = ItemType.None;

        if(ItemCatalogManager.Instance.TryGetItemData(dragID,out var data))
        {
            dragType = data.Type;
        }
        if (ItemCatalogManager.Instance.TryGetItemData(dropID,out data))
        {
            dropType = data.Type;
        }
        if((dragType == dropType) || (dragType == ItemType.Helmet && dropType == ItemType.None))
        {
            return true;
        }
        else if ((dragType == dropType) || (dragType == ItemType.Weapon && dropType == ItemType.None))
        {
            return true;
        }
        else if ((dragType == dropType) || (dragType == ItemType.Body && dropType == ItemType.None))
        {
            return true;
        }
        else if ((dragType == dropType) || (dragType == ItemType.Pents && dropType == ItemType.None))
        {
            return true;
        }
        else if ((dragType == dropType) || (dragType == ItemType.Shoes && dropType == ItemType.None))
        {
            return true;
        }
        else if ((dragType == dropType) || (dragType == ItemType.BackPack && dropType == ItemType.None))
        {
            return true;
        }
        return false;
    }
}
