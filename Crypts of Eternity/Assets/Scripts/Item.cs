using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Weapon,
    Armor,
    Amulet,
    Consumable,
    Misc
}
public class Item : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int rarity;
    public string description;
    public int value;
    public GameObject itemPrefab;

    public Item(string name, Sprite icon)
    {
        itemName = name;
        this.icon = icon;
    }
    public Item item;

    private void Start()
    {
        item = GetComponent<Item>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager inventoryManager = collision.GetComponent<InventoryManager>();

            if (inventoryManager != null)
            {
                if (inventoryManager.AddItem(item))
                {
                    Debug.Log($"Picked up {item.itemName}");
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Inventory is full! Could not pick up item.");
                }
            }
        }
    }
}
