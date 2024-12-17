using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    public string[] possibleLoot = { "Gold", "Potion", "Sword" };

    void Start()
    {
        // Randomly choose loot (example)
        string selectedLoot = possibleLoot[Random.Range(0, possibleLoot.Length)];
        Debug.Log($"Loot generated: {selectedLoot}");
    }
}
