using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehavior : MonoBehaviour
{
    public ShopItemPool itemPool;
    public List<Item> displayedItems = new List<Item>();
    private PlayerController player;
    public InventoryManager inventory;
    public int numberOfItemsToDisplay = 3;
    public List<ShopItemUI> itemUIElements;

    public void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
            return;
        }

        if (itemPool == null)
        {
            Debug.LogError("Item pool is not assigned. Please assign it in the Inspector.");
            return;
        }

        Item key = GetSpecificItem();
        DisplayRandomItems(key);
    }

    public void DisplayRandomItems(Item key)
    {
        displayedItems.Clear();
        displayedItems.Add(null);
        displayedItems.Add(key);
        displayedItems.Add(null);
        List<Item> poolCopy = new List<Item>(itemPool.availableItems);

        // Randomly select items based on rarity
        for (int i = 0; i < numberOfItemsToDisplay; i++)
        {
        if (displayedItems[i] == null && poolCopy.Count > 0)
            {
                Item randomItem = GetRandomItemBasedOnRarity(poolCopy);
                displayedItems[i] = randomItem;
                poolCopy.Remove(randomItem);
            }
        }

        UpdateUI();
    }

        public void OnPurchaseButtonClicked(int index)
    {
        if (index >= 0 && index < displayedItems.Count)
        {
            Item itemToPurchase = displayedItems[index];
            PurchaseItem(itemToPurchase, index);
        }
    }


    public void PurchaseItem(Item item, int index)
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned. Cannot purchase item.");
            return;
        }

        if (player.coins >= item.value && !inventory.inventoryFull)
        {
            AddItemToInventory(item);
            Debug.Log("Purchased: " + item.itemName);

            ReplaceItemInSlot(index);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to purchase: " + item.itemName + " or inventory is full");
        }
    }

    public void AddItemToInventory(Item item)
    {
        if (inventory != null)
        {
            if (!inventory.inventoryFull)
            {
                inventory.AddItem(item);
                player.SpendMoney(item.value);
                Debug.Log("Added " + item.itemName + " to inventory.");
            }
        }
        else
        {
            Debug.Log("Inventory not found");
        }
    }

    public Item GetRandomItemBasedOnRarity(List<Item> pool)
    {
        // Calculate the total weight based on item rarity
        int totalWeight = 0;
        foreach (Item item in pool)
        {
            totalWeight += GetWeightForRarity(item.rarity);
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (Item item in pool)
        {
            cumulativeWeight += GetWeightForRarity(item.rarity);
            if (randomValue < cumulativeWeight)
            {
                return item;
            }
        }

        return null;
    }

    public int GetWeightForRarity(int rarity)
    {
        return Mathf.Max(1, rarity * 10);
    }

    public Item GetSpecificItem()
    {
        return itemPool.availableItems[19]; //Item number of key
    }

    private void ReplaceItemInSlot(int index)
{
    List<Item> poolCopy = new List<Item>(itemPool.availableItems);
    Item randomItem = GetRandomItemBasedOnRarity(poolCopy);
    displayedItems[index] = randomItem;
}

    public void UpdateUI()
    {
        for (int i = 0; i < itemUIElements.Count; i++)
        {
            if (i < displayedItems.Count)
            {
                itemUIElements[i].SetItem(displayedItems[i], this);
            }
            else
            {
                itemUIElements[i].ClearItem();
            }
        }
    }
}