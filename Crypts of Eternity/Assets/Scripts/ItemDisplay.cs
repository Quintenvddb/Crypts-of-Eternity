using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public ItemScriptableObject item;
    public SpriteRenderer spriteRenderer;

    public void Setup(ItemScriptableObject newItem)
    {
        item = newItem;
        spriteRenderer.sprite = item.icon;
        // Additional setup logic
    }
}