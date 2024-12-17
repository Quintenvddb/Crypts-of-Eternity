using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public int maxStackSize = 1;

    public Item(string name, Sprite icon, int stackSize = 1)
    {
        itemName = name;
        this.icon = icon;
        maxStackSize = stackSize;
    }
}

public class InventoryManager : MonoBehaviour
{
    public PlayerController playerController;
    private bool inventoryToggled = false;
    public GameObject inventoryUI;
    public GameObject inventoryGrid;
    public GameObject equipmentSlots;

    private const int inventoryRows = 3;
    private const int inventoryColumns = 8;
    public Sprite slotTexture;
    public float slotSpacing = 8f;

    public List<Item> inventory = new List<Item>();
    public int maxInventorySize = 24;

    private Item[] inventoryItems;

    void Start()
    {
        inventoryUI.SetActive(inventoryToggled);
        InitializeInventory();
        InitializeEquipmentSlots();
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            ToggleInventory();
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = item;
                UpdateSlot(i);
                return true;
            }
        }
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
        if (fromIndex < 0 || fromIndex >= inventoryItems.Length ||
            toIndex < 0 || toIndex >= inventoryItems.Length) return;

        Item temp = inventoryItems[fromIndex];
        inventoryItems[fromIndex] = inventoryItems[toIndex];
        inventoryItems[toIndex] = temp;

        UpdateSlot(fromIndex);
        UpdateSlot(toIndex);
    }

    private void UpdateSlot(int slotIndex)
    {
        Transform slot = inventoryGrid.transform.GetChild(slotIndex);
        InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
        inventorySlot.UpdateSlot(inventoryItems[slotIndex]);
    }

    private void InitializeInventory()
    {
        inventoryItems = new Item[inventoryRows * inventoryColumns];

        GridLayoutGroup gridLayout = inventoryGrid.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = inventoryGrid.AddComponent<GridLayoutGroup>();
        }

        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = inventoryColumns;

        for (int i = 0; i < inventoryRows * inventoryColumns; i++)
        {
            GameObject slot = new GameObject($"InventorySlot{i}");
            Image image = slot.AddComponent<Image>();
            if (slotTexture != null)
            {
                image.sprite = slotTexture;
            }
            image.color = new Color(1f, 1f, 1f, 0.95f);

            InventorySlot slotComponent = slot.AddComponent<InventorySlot>();
            slotComponent.slotIndex = i;

            slot.transform.SetParent(inventoryGrid.transform, false);
        }
    }

    private void InitializeEquipmentSlots()
    {
        GridLayoutGroup gridLayout = equipmentSlots.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = equipmentSlots.AddComponent<GridLayoutGroup>();
        }

        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        for (int i = 0; i < 8; i++)
        {
            GameObject slot = new GameObject($"EquipmentSlot{i}");
            Image image = slot.AddComponent<Image>();
            if (slotTexture != null)
            {
                image.sprite = slotTexture;
            }
            image.color = new Color(1f, 1f, 1f, 0.95f);
            slot.transform.SetParent(equipmentSlots.transform, false);
        }
    }

    private void ToggleInventory()
    {
        inventoryToggled = !inventoryToggled;
        inventoryUI.SetActive(inventoryToggled);

        if (inventoryToggled)
        {
            Debug.Log("Inventory on");
            playerController.moveSpeed = 2f;
        }
        else
        {
            Debug.Log("Inventory off");
            playerController.moveSpeed = 4f;
        }
    }
}