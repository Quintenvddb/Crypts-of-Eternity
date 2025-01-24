using System.Collections.Generic;
using UnityEngine;

public class ShopBehavior : MonoBehaviour
{
    public ShopItemPool itemPool;
    public List<Item> displayedItems = new List<Item>();
    private PlayerController player;
    public int numberOfItemsToDisplay = 3;

    public List<ShopItemUI> itemUIElements;

    void Start()
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

        DisplayRandomItems();
    }

    void DisplayRandomItems()
    {
        displayedItems.Clear();
        List<Item> poolCopy = new List<Item>(itemPool.availableItems);

        // Randomly select items based on rarity
        for (int i = 0; i < numberOfItemsToDisplay; i++)
        {
            if (poolCopy.Count == 0) break;

            Item randomItem = GetRandomItemBasedOnRarity(poolCopy);
            if (randomItem != null)
            {
                displayedItems.Add(randomItem);
                poolCopy.Remove(randomItem);
            }
        }

        UpdateUI();
    }

    public void PurchaseItem(Item item)
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned. Cannot purchase item.");
            return;
        }

        if (player.coins >= item.value)
        {
            //player.SpendMoney(item.value);
            AddItemToInventory(item);
            Debug.Log("Purchased: " + item.itemName);
        }
        else
        {
            Debug.Log("Not enough money to purchase: " + item.itemName);
        }
    }

    void AddItemToInventory(Item item)
    {
        // Implement inventory addition logic here
        Debug.Log("Added " + item.itemName + " to inventory.");
    }

    Item GetRandomItemBasedOnRarity(List<Item> pool)
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

    int GetWeightForRarity(int rarity)
    {
        return Mathf.Max(1, rarity * 10);
    }

    void UpdateUI()
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