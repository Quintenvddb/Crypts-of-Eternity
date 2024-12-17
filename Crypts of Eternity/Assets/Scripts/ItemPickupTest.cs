using UnityEngine;

public class ItemPickupTest : MonoBehaviour
{
    public Item itemToGive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController != null)
            {
                InventoryManager inventoryManager = collision.GetComponentInChildren<InventoryManager>();

                if (inventoryManager != null)
                {
                    bool added = inventoryManager.AddItem(itemToGive);

                    if (added)
                    {
                        Debug.Log($"Picked up {itemToGive.itemName}");
                    }
                    else
                    {
                        Debug.Log("Inventory is full!");
                    }
                }
                else
                {
                    Debug.LogError("InventoryManager not found on Player!");
                }
            }
        }
    }
}
