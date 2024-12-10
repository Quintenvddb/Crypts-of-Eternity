using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        // Simple patrol movement (example logic)
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}