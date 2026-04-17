using System;
using UnityEngine;
[DefaultExecutionOrder(-999)]
public class PlayerBaseEquipment : MonoBehaviour
{
    public static PlayerBaseEquipment Instance;
    public string WeaponID;
    public string HeadArmorID;
    public string BodyArmorID { get; private set; }
    public string ShoesArmorID { get; private set; }
    public string PentsArmorID { get; private set; }

    public Action OnChangeArmor;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UnEquipArmor(int index)
    {
        if (index == 0) HeadArmorID = string.Empty;
        else if (index == 1) BodyArmorID = string.Empty;
        else if (index == 2) PentsArmorID = string.Empty;
        else if (index == 3) ShoesArmorID = string.Empty;
        else if (index == 4) WeaponID = string.Empty;

        OnChangeArmor?.Invoke();
    }
    public void SetArmor(string armorID, out string backItemID)
    {
        backItemID = string.Empty;
        
        ItemCatalogManager.Instance.TryGetItemData(armorID, out var data);


        switch (data.Type)
        {
            case ItemType.MeleeWeapon:
            case ItemType.RangeWeapon: backItemID = WeaponID; WeaponID = armorID; break;
            case ItemType.Helmet: backItemID = HeadArmorID; HeadArmorID = armorID; break;
            case ItemType.Body: backItemID = BodyArmorID; BodyArmorID = armorID; break;
            case ItemType.Pents: backItemID = PentsArmorID; PentsArmorID = armorID; break;
            case ItemType.Shoes: backItemID = ShoesArmorID; ShoesArmorID = armorID;break;
        }
        OnChangeArmor?.Invoke();
    }
}
