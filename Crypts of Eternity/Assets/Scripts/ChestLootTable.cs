using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestLootTable", menuName = "Loot System/Chest Loot Table")]
public class ChestLootTable : ScriptableObject
{
    public List<Item> lootItems;

    public Item GetRandomItem()
    {
        int totalWeight = 0;
        foreach (var Item in lootItems)
        {
            totalWeight += Item.rarity;
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (var Item in lootItems)
        {
            cumulativeWeight += Item.rarity;
            if (randomValue < cumulativeWeight)
            {
                return Item;
            }
        }

        return null; // In case the list is empty
    }

}
