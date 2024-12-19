using System.Collections.Generic;
using Assets.Resources.Scripts;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public ItemPool itemPool;
    public List<GameObject> lootPrefabs; // List of loot prefabs
    public Transform spawnPoint;        // Where to spawn the loot


    public void Start()
    {
        if (itemPool == null)
        {
            Debug.LogError("ItemPool is not assigned!");
            return;
        }

        if (itemPool.itemList == null)
        {
            Debug.LogError("ItemPool's itemList is null!");
            return;
        }

        if (itemPool.itemList.Count == 0)
        {
            Debug.LogWarning("ItemPool's itemList is empty!");
            return;
        }

    int selectedLoot = Random.Range(0, itemPool.itemList.Count);
    Debug.Log($"Loot generated: {selectedLoot}");
    }

    public void GenerateLoot()
    {
        if (lootPrefabs == null || lootPrefabs.Count == 0)
        {
            Debug.LogError("Loot list is empty!");
            return;
        }

        int selectedIndex = Random.Range(0, lootPrefabs.Count);
        GameObject selectedLoot = lootPrefabs[selectedIndex];
        
        if (selectedLoot != null && spawnPoint != null)
        {
            Instantiate(selectedLoot, spawnPoint.position, Quaternion.identity);
            Debug.Log($"Generated loot: {selectedLoot.name}");
        }
        else
        {
            Debug.LogError("Selected loot or spawn point is null!");
        }
    }
}