using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public int attackDamage = 5;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 smoothMovement;
    private Animator animator;

    private int attackState = 1;
    private bool isAttacking = false;

    public int maxHealth = 100;
    public int currentHealth;

    private bool isDead = false;

    public Collider2D attackCollider;

    private Vector3 defaultScale;

    public AudioSource audioSource;
    public AudioClip attackAudio;
    public AudioClip blockAudio;
    public AudioClip hurtAudio;
    public AudioClip healAudio;
    public AudioClip deathAudio;
    public float blockVolume = 1.0f;
    public float attackVolume = 2.0f;
    public float hurtVolume = 1.0f;
    public float healVolume = 1.0f;
    public float stepVolume = 4.0f;
    public float deathVolume = 4.0f;

    public AudioClip[] footstepSounds;
    private bool isPlayingFootstep = false;

    private Vector2 knockbackVelocity;
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer = 0f;

    private bool isBlocking = false;

    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public GameObject deathMenu;

    public int coins = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        defaultScale = transform.localScale;
    }

    void Update()
    {
        if (isDead) return;

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                knockbackVelocity = Vector2.zero;
            }
        }
        else
        {
            HandleMovementInput();
            HandleActions();
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (isKnockedBack)
            {
                rb.linearVelocity = knockbackVelocity;
            }
            else
            {
                smoothMovement = Vector2.Lerp(smoothMovement, movement.normalized * moveSpeed, 0.2f);
                rb.linearVelocity = smoothMovement;
                HandleFootsteps();
            }
        }
    }

    private void HandleMovementInput()
    {
        movement.x = (Input.GetKey(moveRightKey) ? 1 : 0) + (Input.GetKey(moveLeftKey) ? -1 : 0);
        movement.y = (Input.GetKey(moveUpKey) ? 1 : 0) + (Input.GetKey(moveDownKey) ? -1 : 0);

        if (movement.x != 0 && Mathf.Sign(transform.localScale.x) != Mathf.Sign(movement.x))
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x) * defaultScale.x, defaultScale.y, defaultScale.z);
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

public void RebindKey(string action, KeyCode newKey)
{
    switch (action)
    {
        case "MoveUp":
            moveUpKey = newKey;
            break;
        case "MoveDown":
            moveDownKey = newKey;
            break;
        case "MoveLeft":
            moveLeftKey = newKey;
            break;
        case "MoveRight":
            moveRightKey = newKey;
            break;
    }
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
            audioSource.PlayOneShot(blockAudio, blockVolume);
            isBlocking = true;
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
            isBlocking = false;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        switch (attackState)
        {
            case 1:
                animator.SetTrigger("Attack1");
                audioSource.PlayOneShot(attackAudio, attackVolume);
                attackState = 2;
                break;
            case 2:
                animator.SetTrigger("Attack2");
                audioSource.PlayOneShot(attackAudio, attackVolume);
                attackState = 3;
                break;
            case 3:
                animator.SetTrigger("Attack3");
                audioSource.PlayOneShot(attackAudio, attackVolume);
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

        int finalDamage = isBlocking ? Mathf.CeilToInt(damage * 0.25f) : damage;

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        audioSource.PlayOneShot(hurtAudio, hurtVolume);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = originalColor;
        }
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        audioSource.PlayOneShot(healAudio, healVolume);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        knockbackVelocity = direction * force;
    }

    private void TriggerDeath()
    {
        isDead = true;
        audioSource.PlayOneShot(deathAudio, deathVolume);
        animator.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 2f);
        deathMenu.SetActive(true);
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
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepSounds[randomIndex], stepVolume);
        }

        yield return new WaitForSeconds(0.4f);

        isPlayingFootstep = false;
    }

    public void SpendMoney(int amount){
        this.coins -= amount;

        return;
    }
}
