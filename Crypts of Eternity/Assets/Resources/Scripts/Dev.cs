using UnityEngine;

public class Dev : MonoBehaviour
{
    public GameObject Player;
    public GameObject DevPlayground;

    void Start()
    {
        DevPlayground.SetActive(false);
        if (Player == null)
        {
            Debug.LogWarning("Player GameObject is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Player != null)
        {
            DevPlayground.SetActive(true);
            Player.transform.position = new Vector3(298, 300, 0);
            Debug.Log($"Moved {Player.name} to position (298, 300, 0)");
        }
    }
}
