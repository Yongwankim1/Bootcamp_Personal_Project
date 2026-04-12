using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] Inventory inventory;
    [SerializeField] Transform slotParent;
    [SerializeField] SlotUI slotUIPrefab;

    [SerializeField] GameObject enemyInventoryUI;

    private SlotUI[] slots;
    private SlotUI[] enemySlots;


    private void Awake()
    {
        if (inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        if(inputReader == null) inputReader = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputReader>();
        slots = new SlotUI[0];
        enemySlots = new SlotUI[10];
    }
    private void OnEnable()
    {
        if (inputReader != null) inputReader.DontAction = true;
        enemyInventoryUI.SetActive(false);
        if(inventory != null)
        {
            inventory.OnBackPackChanage += BackPackSlotChanage;
            inventory.OnChangeItem += DrawAllSlot;
        }
        if(slots.Length == 0) BackPackSlotChanage();
        DrawAllSlot();
    }

    private void OnDisable()
    {
        if (inputReader != null) inputReader.DontAction = false;
        if (inventory != null)
        {
            inventory.OnBackPackChanage -= BackPackSlotChanage;
            inventory.OnChangeItem -= DrawAllSlot;
        }
        if(enemyInventoryUI != null)
        {
            enemyInventoryUI.SetActive(false);
        }
    }
    public void OnEnemyInventory()
    {
        if (enemyInventoryUI == null)
        {
            Debug.LogWarning("적 인벤토리 참조 안됨");
            return;
        }
        enemyInventoryUI.SetActive(true);
    }
    private void BackPackSlotChanage()
    {
        if (inventory == null) return;
        slots = new SlotUI[inventory.ItemPositiones.Length];
        DrawAllSlot();
    }

    void DrawAllSlot()
    {
        if (inventory == null) return;
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = Instantiate(slotUIPrefab, slotParent);
                slots[i].Initialize(inventory.ItemPositiones[i].ItemID, inventory.ItemPositiones[i].Amount,this);
            }
            else
            {
                slots[i].Initialize(inventory.ItemPositiones[i].ItemID, inventory.ItemPositiones[i].Amount, this);
            }
        }
    }



}
