using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestLootTable lootTable;
    public Transform spawnPoint;
    private Animator animator;
    private bool isOpened = false;
    public AudioSource audioSource;
    public AudioClip chestAudio;
    public float chestVolume = 1.0f;

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
        audioSource.PlayOneShot(chestAudio, chestVolume);
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
