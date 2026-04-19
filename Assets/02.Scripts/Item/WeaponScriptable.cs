using UnityEngine;


[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/WeaponItem")]
public class WeaponScriptable : ItemScriptable, IAttack
{

    public void Attack(Transform attackTransform, float attackDistance, Vector3 mouseWorldPos, LayerMask targetLayer)
    {
        Vector3 dir = mouseWorldPos - attackTransform.position;
        Vector3 attackPosition = attackTransform.position + dir.normalized * attackDistance;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        GameObject go = PoolingManager.Instance.Get(ItemData.WeaponEffect);
        if (go == null)
        {
            go = Instantiate(ItemData.WeaponEffect);
            go.GetComponent<HitBox>().Initialize(ItemData.Damage, targetLayer);
            go.name = ItemData.WeaponEffect.name;
        }
        go.GetComponent<HitBox>().Initialize(ItemData.Damage, targetLayer);
        go.transform.position = attackPosition;
        go.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        go.SetActive(true);
    }
    public override void Use(Inventory inventory)
    {
        if (PlayerBaseEquipment.Instance == null) return;
        Debug.Log(ItemData.ItemID);
        PlayerBaseEquipment.Instance.Equip(ItemData.ItemID, out string backItemID);
        inventory.AddItem(backItemID, 1);
    }
}
