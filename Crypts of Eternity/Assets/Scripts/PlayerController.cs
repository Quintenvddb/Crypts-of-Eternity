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

    private Vector3 defaultScale;

    public AudioSource audioSource;
    public AudioClip attackAudio;
    public AudioClip blockAudio;
    public float volume = 2.0f;
    public float stepVolume = 4.0f;

    public AudioClip[] footstepSounds;  // Array for footstep sounds
    private bool isPlayingFootstep = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        defaultScale = transform.localScale;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        HandleMovementInput();
        HandleActions();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            rb.linearVelocity = movement.normalized * moveSpeed;
            HandleFootsteps();
        }
    }

    private void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0 && Mathf.Sign(transform.localScale.x) != Mathf.Sign(movement.x))
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x) * defaultScale.x, defaultScale.y, defaultScale.z);
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Block");
            audioSource.PlayOneShot(blockAudio, volume);
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

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        switch (attackState)
        {
            case 1:
                animator.SetTrigger("Attack1");
                audioSource.PlayOneShot(attackAudio, volume);
                attackState = 2;
                break;
            case 2:
                animator.SetTrigger("Attack2");
                audioSource.PlayOneShot(attackAudio, volume);
                attackState = 3;
                break;
            case 3:
                animator.SetTrigger("Attack3");
                audioSource.PlayOneShot(attackAudio, volume);
                attackState = 1;
                break;
        }

        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        yield return new WaitForSeconds(GetAttackAnimationLength());

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }

        isAttacking = false;
    }

    private float GetAttackAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        animator.SetTrigger("Hurt");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void TriggerDeath()
    {
        isDead = true;
        animator.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
        }
    }

    private void HandleFootsteps()
    {
        if (movement.sqrMagnitude > 0.1f && !isAttacking && !isPlayingFootstep)  
        {
            StartCoroutine(PlayFootstep());
        }
    }

    private IEnumerator PlayFootstep()
    {
        isPlayingFootstep = true;

        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // Random pitch for variation
            audioSource.PlayOneShot(footstepSounds[randomIndex], stepVolume);
        }

        yield return new WaitForSeconds(0.4f); // Adjust footstep timing

        isPlayingFootstep = false;
    }
}
