using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item itemData;

    void Start()
    {
        Debug.Log("Item: " + itemData.itemName);

        if (itemData is Weapon weapon)
        {
            Debug.Log("Weapon Damage: " + weapon.damage);
            // Apply weapon-specific logic
        }
        else if (itemData is Armor armor)
        {
            Debug.Log("Armor Defense: " + armor.defense);
            // Apply armor-specific logic
        }
        else if (itemData is Consumable consumable)
        {
            Debug.Log("Consumable Effect: " + consumable.effect);
            // Apply consumable-specific logic
        }
    }
}
