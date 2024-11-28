using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private float idleTimer = 0f;
    public float idleTimeThreshold = 10f;
    public float sitTimeThreshold = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x) * 8, 8, 1);
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude == 0)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
        }

        if (idleTimer >= sitTimeThreshold)
        {
            animator.SetInteger("idle", 2);
        }
        else if (idleTimer >= idleTimeThreshold)
        {
            animator.SetInteger("idle", 1);
        }
        else
        {
            animator.SetInteger("idle", 0);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movement.normalized * moveSpeed, 0.1f);
    }
}
