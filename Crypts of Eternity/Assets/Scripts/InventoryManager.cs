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

    private const int inventoryRows = 3;
    private const int inventoryColumns = 8;

    public GameObject optionsWindow;
    public Button useButton;
    public Button equipButton;
    private Item selectedItem;

    private bool inventoryToggled;
    public Item[] inventoryItems;
    public Item[] equipmentItems;
    public bool inventoryFull = false;

    void Start()
    {
        inventoryUI.SetActive(inventoryToggled);
        InitializeInventory();
        InitializeEquipmentSlots();

        equipmentItems = new Item[8];

        useButton.onClick.AddListener(OnUseItem);
        equipButton.onClick.AddListener(OnEquipItem);
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
        inventoryFull = true;
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
        playerController.IsInventoryOpen = inventoryToggled; // Update the player's inventory state
        playerController.moveSpeed = inventoryToggled ? 2f : 4f;
    }

    public void ShowItemOptions(Item item, Vector3 position)
    {
        selectedItem = item;
        optionsWindow.SetActive(true);
        optionsWindow.transform.position = position;
    }

    private void OnUseItem()
    {
        if (selectedItem != null)
        {
            UseItem(selectedItem);
        }
        optionsWindow.SetActive(false);
    }

    private void OnEquipItem()
    {
        if (selectedItem != null)
        {
            EquipItem(selectedItem);
        }
        optionsWindow.SetActive(false);
    }

    private void UseItem(Item item)
    {
        if (item is Consumable consumable)
        {
            if (playerController.currentHealth != playerController.maxHealth)
            {
                ApplyConsumableEffects(consumable);
                RemoveItemFromInventory(item);
                Debug.Log($"Used consumable: {item.itemName}");
            }
        }
    }

    private void ApplyConsumableEffects(Consumable consumable)
    {
        if (consumable.restoreAmount > 0)
        {
            playerController.currentHealth += consumable.restoreAmount;
            playerController.currentHealth = Mathf.Min(playerController.currentHealth, playerController.maxHealth);
        }
    }

    private void EquipItem(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            if (equipmentItems[0] == null)
            {
                equipmentItems[0] = item;
                RemoveItemFromInventory(item);
                Debug.Log($"Equipped weapon: {item.itemName}");
                UpdateEquipmentSlot(0);
                ApplyItemStats(item);
            }
        }
        else if (item.itemType == ItemType.Armor)
        {
            if (equipmentItems[1] == null)
            {
                equipmentItems[1] = item;
                RemoveItemFromInventory(item);
                Debug.Log($"Equipped armor: {item.itemName}");
                UpdateEquipmentSlot(1);
                ApplyItemStats(item);
            }
        }
        else if (item.itemType == ItemType.Amulet)
        {
            for (int i = 2; i <= 4; i++)
            {
                if (equipmentItems[i] == null)
                {
                    equipmentItems[i] = item;
                    RemoveItemFromInventory(item);
                    Debug.Log($"Equipped amulet: {item.itemName} in slot {i}");
                    UpdateEquipmentSlot(i);
                    ApplyItemStats(item);
                    return;
                }
            }
        }
    }

    private void ApplyItemStats(Item item)
    {
        if (item is Weapon weapon)
        {
            playerController.attackDamage += weapon.damage;
            playerController.attackSpeed += weapon.attackSpeed;
        }
        else if (item is Armor armor)
        {
            playerController.maxHealth += armor.defense;
        }
        else if (item is Amulet amulet)
        {
            playerController.attackDamage += amulet.damage;
            playerController.attackSpeed += amulet.attackSpeed;
            playerController.maxHealth += amulet.defense;
        }
    }

    private void RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == item)
            {
                inventoryItems[i] = null;
                UpdateSlot(i);
                break;
            }
        }
    }

    private void UpdateEquipmentSlot(int slotIndex)
    {
        Transform slot = equipmentSlots.transform.GetChild(slotIndex);
        InventorySlot equipmentSlot = slot.GetComponent<InventorySlot>();

        Item equippedItem = equipmentItems[slotIndex];
        equipmentSlot.UpdateSlot(equippedItem);
    }
}
