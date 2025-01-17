using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLootTable", menuName = "Loot System/Enemy Loot Table")]
public class EnemyLootTable : ScriptableObject
{
    public List<GameObject> lootItems;

    public GameObject GetRandomLoot()
    {
        if (lootItems == null || lootItems.Count == 0)
            return null;

        int randomIndex = Random.Range(0, lootItems.Count);
        return lootItems[randomIndex];
    }
}
