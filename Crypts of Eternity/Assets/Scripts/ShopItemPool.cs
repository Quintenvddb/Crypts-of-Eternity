using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemPool", menuName = "Shop/Item Pool")]
public class ShopItemPool : ScriptableObject
{
    public List<Item> availableItems;
}
