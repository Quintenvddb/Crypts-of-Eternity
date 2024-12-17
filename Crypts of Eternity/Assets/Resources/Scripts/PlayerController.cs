using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public int attackDamage = 5;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private int attackState = 1;
    private bool isAttacking = false;

    public int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;

    public Slider healthSlider;

    public Collider2D attackCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        HealthChanged();
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            TriggerDeath();
            return;
        }

        if (isDead) return;

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

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Block");
        }

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("IdleBlock", true);
            moveSpeed = 1f;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IdleBlock", false);
            moveSpeed = 4f;
        }
    }

    void FixedUpdate()
    {
        if (currentHealth > 0 && !isDead)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movement.normalized * moveSpeed, 0.1f);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
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

        StartCoroutine(ToggleAttackCollider());

        yield return new WaitForSeconds(GetAttackAnimationLength());

        isAttacking = false;
    }

    private float GetAttackAnimationLength()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0 && !isDead)
        {
            int previousHealth = currentHealth;
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            if (previousHealth > currentHealth)
            {
                animator.SetTrigger("Hurt");
            }

            HealthChanged();
        }
    }

    private void TriggerDeath()
    {
        isDead = true;
        animator.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 2f);
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        HealthChanged();
    }

    private void HealthChanged()
    {
        Debug.Log("Player Health: " + currentHealth + "/" + maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private IEnumerator ToggleAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            yield return new WaitForSeconds(1f);
            attackCollider.enabled = false;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
    //         if (enemy != null)
    //         {
    //             enemy.TakeDamage(attackDamage);
    //         }
    //     }
    // }
}