using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item item;
    public SpriteRenderer spriteRenderer;

    public void Setup(Item newItem)
    {
        item = newItem;
        spriteRenderer.sprite = item.icon;
        // Additional setup logic
    }
}