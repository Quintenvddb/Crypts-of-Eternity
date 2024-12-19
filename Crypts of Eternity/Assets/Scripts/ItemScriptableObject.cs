using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float rarity;
    public int value;
    public string description;
    public GameObject itemPrefab;
}
