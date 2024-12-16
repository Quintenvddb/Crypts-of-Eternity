using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    private Image slotImage;
    private Item currentItem;
    private InventoryManager inventoryManager;

    private GameObject dragIcon;
    private Canvas canvas;

    void Awake()
    {
        slotImage = GetComponent<Image>();
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();
        canvas = Object.FindFirstObjectByType<Canvas>();
    }

    public void UpdateSlot(Item item)
    {
        currentItem = item;
        if (currentItem != null)
        {
            slotImage.sprite = currentItem.icon;
            slotImage.color = Color.white;
        }
        else
        {
            slotImage.sprite = inventoryManager.slotTexture;
            slotImage.color = new Color(1f, 1f, 1f, 0.95f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        dragIcon = new GameObject("DragIcon");
        Image dragImage = dragIcon.AddComponent<Image>();
        dragImage.sprite = slotImage.sprite;
        dragImage.raycastTarget = false;

        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();
        RectTransform rectTransform = dragIcon.GetComponent<RectTransform>();
        rectTransform.sizeDelta = slotImage.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        GameObject droppedOn = eventData.pointerEnter;
        if (droppedOn != null && droppedOn.GetComponent<InventorySlot>() != null)
        {
            InventorySlot targetSlot = droppedOn.GetComponent<InventorySlot>();
            inventoryManager.SwapItems(slotIndex, targetSlot.slotIndex);
        }
    }
}
