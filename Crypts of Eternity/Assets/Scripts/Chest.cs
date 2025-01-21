using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestLootTable lootTable;
    public Transform spawnPoint;
    private Animator animator;
    private bool isOpened = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        animator.SetTrigger("ChestOpenAnim");
    }

    public void AnimationComplete()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        GetComponent<SpriteRenderer>().color = Color.gray;

        SpawnItem();

        Debug.Log("Spawn Position: " + spawnPoint.position);
    }

    private void SpawnItem()
    {
        Item selectedItem = lootTable.GetRandomItem();
        if (selectedItem != null && selectedItem.itemPrefab != null)
        {
            Instantiate(selectedItem.itemPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
