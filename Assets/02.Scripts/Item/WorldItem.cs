using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItem : MonoBehaviour, IInteractable
{
    [SerializeField] string itemID;
    [SerializeField] int amount;
    [SerializeField] SpriteRenderer spriteRenderer;
    public void Init(string itemID, int amount, Sprite sprite)
    {
        this.itemID = itemID;
        this.amount = amount;
        spriteRenderer.sprite = sprite;
    }
    public void Interact(PlayerInteract player)
    {
        Inventory inventory = player.GetComponentInParent<Inventory>();
        if (inventory == null) return;
        PickUP(inventory);
    }

    private void PickUP(Inventory inventory)
    {
        amount = inventory.AddItem(itemID, amount);
        Debug.Log("아이템 픽업됨");
        if(amount <= 0) Destroy(gameObject);
    }
}
