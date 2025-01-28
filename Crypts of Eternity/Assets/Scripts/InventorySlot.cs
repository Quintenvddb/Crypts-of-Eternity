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
        slotImage.sprite = currentItem?.icon ?? inventoryManager.slotTexture;
        slotImage.color = currentItem != null ? Color.white : new Color(1f, 1f, 1f, 0.95f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        dragIcon = CreateDragIcon();
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
        Destroy(dragIcon);

        if (eventData.pointerEnter is GameObject droppedOn && droppedOn.GetComponent<InventorySlot>() != null)
        {
            InventorySlot targetSlot = droppedOn.GetComponent<InventorySlot>();
            inventoryManager.SwapItems(slotIndex, targetSlot.slotIndex);
        }
    }

    private GameObject CreateDragIcon()
    {
        GameObject icon = new GameObject("DragIcon");
        Image dragImage = icon.AddComponent<Image>();
        dragImage.sprite = slotImage.sprite;
        dragImage.raycastTarget = false;
        icon.transform.SetParent(canvas.transform, false);
        icon.transform.SetAsLastSibling();
        icon.GetComponent<RectTransform>().sizeDelta = slotImage.rectTransform.sizeDelta;
        return icon;
    }
}
