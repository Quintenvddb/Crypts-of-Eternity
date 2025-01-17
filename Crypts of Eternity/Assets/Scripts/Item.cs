using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string type;
    public int rarity;
    public string description;
    public int damage;
    public int value;

    public Item(string name, Sprite icon)
    {
        itemName = name;
        this.icon = icon;
    }
}
