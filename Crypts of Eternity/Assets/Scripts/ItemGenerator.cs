using Assets.Resources.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemGenerator : MonoBehaviour
{
    public ItemPool itemPool;
    public Vector3 scale = new Vector3(10,10,10);

    public ItemScriptableObject GenerateRandomItem()
    {
        if (itemPool.itemList.Count == 0) return null;

        List<ItemScriptableObject> validItems = itemPool.itemList;

        int randomIndex = Random.Range(0, validItems.Count);
        return itemPool.itemList[randomIndex];
    }

    public void SpawnItem(Vector3 position)
    {
        var generatedItem = GenerateRandomItem();
        if (generatedItem != null)
        {
            GameObject itemObject = Instantiate(generatedItem.itemPrefab, position, Quaternion.identity);
            // Assuming the prefab has a script to display item details
            itemObject.transform.localScale = scale;
            itemObject.GetComponent<ItemDisplay>().Setup(generatedItem);
        }
    }
}
