using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string type;
    public int rarity;
    public string description;
    public int value;
    public int maxStackSize = 1;

    public Item(string name, Sprite icon, int stackSize = 1)
    {
        itemName = name;
        this.icon = icon;
        maxStackSize = stackSize;
    }
}
