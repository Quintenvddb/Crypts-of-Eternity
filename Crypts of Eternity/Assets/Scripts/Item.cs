using UnityEngine;

[System.Serializable]
public abstract class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int rarity;
    public string description;
    public int value;
    public GameObject itemPrefab;

    public Item(string name, Sprite icon)
    {
        itemName = name;
        this.icon = icon;
    }
}
