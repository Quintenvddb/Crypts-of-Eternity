using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Text itemNameText;
    public Image itemIconImage;
    public Text itemPriceText;
    private Item currentItem;
    private ShopBehavior shopBehavior;

    public void SetItem(Item item, ShopBehavior shop)
    {
        currentItem = item;
        shopBehavior = shop;

        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.icon;
        itemPriceText.text = item.value.ToString();
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        currentItem = null;
        shopBehavior = null;
        
        itemNameText.text = "";
        itemIconImage.sprite = null;
        itemPriceText.text = "";
        gameObject.SetActive(false);
    }
}
