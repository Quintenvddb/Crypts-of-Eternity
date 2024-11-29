using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private int attackState = 1;
    private bool isAttacking = false;

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
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movement.normalized * moveSpeed, 0.1f);
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        if (attackState == 1)
        {
            animator.SetTrigger("Attack1");
            attackState = 2;
        }
        else if (attackState == 2)
        {
            animator.SetTrigger("Attack2");
            attackState = 3;
        }
        else if (attackState == 3)
        {
            animator.SetTrigger("Attack3");
            attackState = 1;
        }

        yield return new WaitForSeconds(GetAttackAnimationLength());

        isAttacking = false;
    }

    private float GetAttackAnimationLength()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
}
