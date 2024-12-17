using UnityEngine;

public class ShopBehavior : MonoBehaviour
{
    public string[] itemsForSale = { "Health Potion", "Mana Potion", "Sword Upgrade" };

    void Start()
    {
        Debug.Log("Shop is open!");
        foreach (string item in itemsForSale)
        {
            Debug.Log($"Item for sale: {item}");
        }
    }
}
