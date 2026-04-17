using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequimentSlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI amountText;

    public void Init(string itemid, int amount)
    {
        iconImage.enabled = false;
        amountText.text = string.Empty;
        if (!ItemCatalogManager.Instance.TryGetItemData(itemid, out var data))
        {
            gameObject.SetActive(false);
            Debug.Log("데이터 없음");
            return;
        }
        iconImage.enabled = true;
        amountText.text = "X " + amount.ToString();
        iconImage.sprite = data.ItemIcon;
        gameObject.SetActive(true);
    }
}
