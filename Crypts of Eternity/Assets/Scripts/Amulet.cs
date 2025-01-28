using UnityEngine;

public class Amulet : Item
{
    public int damage;
    public float attackSpeed;
    public int defense;

    public Amulet(string name, Sprite icon) : base(name, icon)
    {
    }
}