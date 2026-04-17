using UnityEngine;

public class EquipSlot : SlotUI
{
    [SerializeField] ItemType equiType;
    private void Start()
    {
        Initialize();
    }
    private void OnEnable()
    {
        if(PlayerBaseEquipment.Instance != null)
        {
            PlayerBaseEquipment.Instance.OnChangeArmor += Initialize;
        }
        //Initialize();
    }
    void Initialize()
    {
        if (PlayerBaseEquipment.Instance == null || ItemCatalogManager.Instance == null) return;
        iconImage.enabled = false;
        SlotData.ItemID = GetData();
        if(!ItemCatalogManager.Instance.TryGetItemData(SlotData.ItemID, out var itemData))
        {
            return;
        }
        SlotData.Amount = 1;
        iconImage.sprite = itemData.ItemIcon;
        iconImage.enabled = true;
    }

    private string GetData()
    {
        return equiType switch
        {
            ItemType.Helmet => PlayerBaseEquipment.Instance.HeadArmorID,
            ItemType.Body => PlayerBaseEquipment.Instance.BodyArmorID,
            ItemType.Pents => PlayerBaseEquipment.Instance.PentsArmorID,
            ItemType.Shoes => PlayerBaseEquipment.Instance.ShoesArmorID,
            ItemType.RangeWeapon => PlayerBaseEquipment.Instance.WeaponID,
            ItemType.MeleeWeapon => PlayerBaseEquipment.Instance.WeaponID,
            ItemType.Weapon => PlayerBaseEquipment.Instance.WeaponID,
            _ => string.Empty
        };
    }
}
