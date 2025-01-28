using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject inventoryUI, inventoryGrid, equipmentSlots;
    public Sprite slotTexture;
    public AudioSource audioSource;
    public AudioClip inventoryOpenAudio;
    public float slotSpacing = 8f, inventoryOpenVolume = 0.25f;
    public int maxInventorySize = 24;

    private bool inventoryToggled;
    public Item[] inventoryItems;

    private const int inventoryRows = 3;
    private const int inventoryColumns = 8;

    void Start()
    {
        inventoryUI.SetActive(inventoryToggled);
        InitializeInventory();
        InitializeEquipmentSlots();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            audioSource.PlayOneShot(inventoryOpenAudio, inventoryOpenVolume);
            ToggleInventory();
        }
    }

    public bool AddItem(Item item)
    {
        if (item == null) return false;

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = item;
                UpdateSlot(i);
                return true;
            }
        }
        Debug.LogWarning("Inventory is full!");
        return false;
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
        {
            inventoryItems[slotIndex] = null;
            UpdateSlot(slotIndex);
        }
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        if (IsValidSlot(fromIndex) && IsValidSlot(toIndex))
        {
            Item temp = inventoryItems[fromIndex];
            inventoryItems[fromIndex] = inventoryItems[toIndex];
            inventoryItems[toIndex] = temp;
            UpdateSlot(fromIndex);
            UpdateSlot(toIndex);
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (Item item in inventoryItems)
        {
            if (item != null && item.itemName == itemName) return true;
        }
        return false;
    }

    private bool IsValidSlot(int index) => index >= 0 && index < inventoryItems.Length;

    private void UpdateSlot(int slotIndex)
    {
        Transform slot = inventoryGrid.transform.GetChild(slotIndex);
        slot.GetComponent<InventorySlot>().UpdateSlot(inventoryItems[slotIndex]);
    }

    private void InitializeInventory()
    {
        inventoryItems = new Item[inventoryRows * inventoryColumns];
        GridLayoutGroup gridLayout = GetOrAddComponent<GridLayoutGroup>(inventoryGrid);
        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = inventoryColumns;

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            CreateInventorySlot(i);
        }
    }

    private void InitializeEquipmentSlots()
    {
        GridLayoutGroup gridLayout = GetOrAddComponent<GridLayoutGroup>(equipmentSlots);
        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        for (int i = 0; i < 8; i++)
        {
            CreateEquipmentSlot(i);
        }
    }

    private void CreateInventorySlot(int index)
    {
        GameObject slot = new GameObject($"InventorySlot{index}");
        SetupSlot(slot, inventoryGrid.transform);
    }

    private void CreateEquipmentSlot(int index)
    {
        GameObject slot = new GameObject($"EquipmentSlot{index}");
        SetupSlot(slot, equipmentSlots.transform);
    }

    private void SetupSlot(GameObject slot, Transform parent)
    {
        Image image = slot.AddComponent<Image>();
        image.sprite = slotTexture ?? image.sprite;
        image.color = new Color(1f, 1f, 1f, 0.95f);

        slot.AddComponent<InventorySlot>().slotIndex = parent.childCount;
        slot.transform.SetParent(parent, false);
    }

    private GridLayoutGroup GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null) component = obj.AddComponent<T>();
        return component as GridLayoutGroup;
    }

    private void ToggleInventory()
    {
        inventoryToggled = !inventoryToggled;
        inventoryUI.SetActive(inventoryToggled);
        playerController.moveSpeed = inventoryToggled ? 2f : 4f;
    }
}
