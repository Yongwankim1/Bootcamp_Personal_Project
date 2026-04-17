using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct DropTable
{
    public string DropItemID;
    public int Amount;
    public float CheckTimer;
    public bool IsCheck;
    public float Percent;
}

public class EnemyInventory : MonoBehaviour, IInteractable
{
    [SerializeField] DropTable[] dropTable = new DropTable[0];
    [SerializeField] EnemyHP enemyHP;
    [SerializeField] Collider2D myColider2D;
    [SerializeField] List<DropTable> dropList = new List<DropTable>();
    [SerializeField] InventoryGUI inventoryGUI;

    public List<DropTable> DropList => dropList;
    void Awake()
    {
        if(enemyHP == null) enemyHP = GetComponent<EnemyHP>();
        if(myColider2D == null) myColider2D = GetComponent<Collider2D>();
        if (inventoryGUI == null) inventoryGUI = FindFirstObjectByType<InventoryGUI>(FindObjectsInactive.Include);
    }
    void OnEnable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnDied += OnTrigger;
            enemyHP.OnDied += RandomDropItem;
        }
    }
    void OnDisable()
    {
        if (enemyHP != null)
        {
            enemyHP.OnDied -= OnTrigger;
            enemyHP.OnDied -= RandomDropItem;
        }
    }
    void OnTrigger()
    {
        myColider2D.isTrigger = true;
    }
    void RandomDropItem()
    {
        for (int i = 0; i < dropTable.Length; i++)
        {
            if (!ItemCatalogManager.Instance.IsRegisteredItem(dropTable[i].DropItemID)) continue;

            if (dropTable[i].Percent <= 0)
            {
                Debug.LogWarning($"{dropTable[i]} 드랍률이 0퍼센트 이하입니다");
                continue;
            }
            float rand = Random.Range(0f, 1f);

            if (rand < dropTable[i].Percent)
            {
                dropList.Add(dropTable[i]);
            }
        }
    }
    public void Interact(PlayerInteract player)
    {
        InventoryOpen(player);
    }

    private void InventoryOpen(PlayerInteract player)
    {
        if (!enemyHP.IsDead) return;
        InventoryAction playerInventoryAction = player.GetComponent<InventoryAction>();

        if (playerInventoryAction == null || inventoryGUI == null) return;
        
        playerInventoryAction.OnInventory();
        inventoryGUI.OnEnemyInventory(this);
    }
    public void ListAddItem(string itemID, int amount)
    {
        DropTable item = default;
        for (int i = 0; i < dropList.Count; i++)
        {
            if (!string.IsNullOrEmpty(dropList[i].DropItemID)) continue;
            
            item.DropItemID = itemID;
            item.Amount = amount;
            dropList[i] = item;
            inventoryGUI.OnEnemyInventory(this);
            return;
            
        }
        item.DropItemID = itemID;
        item.Amount = amount;
        dropList.Add(item);
        inventoryGUI.OnEnemyInventory(this);
    }
    public void ListRemove(int index)
    {
        DropTable item = new DropTable();
        item.IsCheck = true;
        dropList[index] = item;
        inventoryGUI.OnEnemyInventory(this);
    }
}
