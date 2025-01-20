using UnityEngine;

public class Consumable : Item
{
    public int restoreAmount;
    public string effect;

    public Consumable(string name, Sprite icon) : base(name, icon)
    {
    }
}