using System.Collections.Generic;
using UnityEngine;

public class ShopBehavior : MonoBehaviour
{
    public ShopItemPool itemPool;
    public List<Item> displayedItems = new List<Item>();
    public int playerCurrency;
    public PlayerController player;
    public int numberOfItemsToDisplay = 3;

    public List<ShopItemUI> itemUIElements;

    void Start()
    {
        DisplayRandomItems();
    }

    void DisplayRandomItems()
    {
        displayedItems.Clear();
        List<Item> poolCopy = new List<Item>(itemPool.availableItems);
        
        // Filter items based on rarity
        for (int i = 0; i < numberOfItemsToDisplay; i++)
        {
            if (poolCopy.Count == 0) break;

            Item randomItem = GetRandomItemBasedOnRarity(poolCopy);
            displayedItems.Add(randomItem);
            poolCopy.Remove(randomItem);
        }

        UpdateUI();
    }

     public void PurchaseItem(Item item)
    {
        if (playerCurrency > item.value)
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
        // Implement inventory addition logic
    }

    Item GetRandomItemBasedOnRarity(List<Item> pool)
    {
        // Weighting logic based on rarity
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
