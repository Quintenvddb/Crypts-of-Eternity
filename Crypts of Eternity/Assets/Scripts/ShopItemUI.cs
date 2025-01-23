using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Text itemNameText;
    public Image itemIconImage;
    public Text itemPriceText;
    public Button purchaseButton;
    private Item currentItem;
    private ShopBehavior shopBehavior;

    public void SetItem(Item item, ShopBehavior shop)
    {
        currentItem = item;
        shopBehavior = shop;

        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.icon;
        itemPriceText.text = item.value.ToString();

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => shop.PurchaseItem(item));
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        currentItem = null;
        shopBehavior = null;
        
        itemNameText.text = "";
        itemIconImage.sprite = null;
        itemPriceText.text = "";

        purchaseButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
