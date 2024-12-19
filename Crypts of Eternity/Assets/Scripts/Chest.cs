using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<ItemScriptableObject> itemsToSpawn;
    public Transform spawnPoint;
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        GetComponent<SpriteRenderer>().color = Color.gray;

        SpawnItem();

        GetComponent<Collider2D>().enabled = false;

        Debug.Log("Spawn Position: " + spawnPoint.position);
    }

    private void SpawnItem()
    {
        ItemScriptableObject selectedItem = SelectItem();
        if (selectedItem != null && selectedItem.itemPrefab != null)
        {
            Instantiate(selectedItem.itemPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private ItemScriptableObject SelectItem()
    {
        float totalWeight = 0f;
        foreach (var item in itemsToSpawn)
        {
            totalWeight += item.rarity;
        }

        float randomValue = Random.value * totalWeight;
        float cumulativeWeight = 0f;

        foreach (var item in itemsToSpawn)
        {
            cumulativeWeight += item.rarity;
            if (randomValue <= cumulativeWeight)
            {
                return item;
            }
        }

        return null;
    }
}
