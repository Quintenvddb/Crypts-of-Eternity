using UnityEngine;

public class Weapon : Item
{
    public int damage;
    public float attackSpeed;

    public Weapon(string name, Sprite icon) : base(name, icon)
    {
    }
}
