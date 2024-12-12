using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
}

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private int attackState = 1;
    private bool isAttacking = false;

    public int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;

    public Slider healthSlider;

    public GameObject inventoryUI;
    public GameObject inventoryGrid;
    public GameObject equipmentSlots;

    private bool inventoryToggled = false;

    private const int inventoryRows = 3;
    private const int inventoryColumns = 8;

    public Sprite slotTexture;
    public float slotSpacing = 8f;

    private Item[] inventoryItems = new Item[inventoryRows * inventoryColumns];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        inventoryUI.SetActive(inventoryToggled);
        HealthChanged();

        InitializeInventory();
        InitializeEquipmentSlots();
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
        if (Input.GetKeyDown("e"))
        {
            ToggleInventory();
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

    private void ToggleInventory()
    {
        inventoryToggled = !inventoryToggled;
        inventoryUI.SetActive(inventoryToggled);

        if (inventoryToggled)
        {
            Debug.Log("Inventory on");
            moveSpeed = 2f;
        }
        else
        {
            Debug.Log("Inventory off");
            moveSpeed = 4f;
        }
    }

    private void InitializeInventory()
    {
        GridLayoutGroup gridLayout = inventoryGrid.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = inventoryGrid.AddComponent<GridLayoutGroup>();
        }

        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = inventoryColumns;

        for (int i = 0; i < inventoryRows * inventoryColumns; i++)
        {
            GameObject slot = new GameObject($"InventorySlot{i}");
            Image image = slot.AddComponent<Image>();
            if (slotTexture != null)
            {
                image.sprite = slotTexture;
            }
            image.color = new Color(1f, 1f, 1f, 0.95f);
            slot.transform.SetParent(inventoryGrid.transform, false);
        }
    }

    private void InitializeEquipmentSlots()
    {
        GridLayoutGroup gridLayout = equipmentSlots.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = equipmentSlots.AddComponent<GridLayoutGroup>();
        }

        gridLayout.cellSize = new Vector2(60, 60);
        gridLayout.spacing = new Vector2(slotSpacing, slotSpacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        for (int i = 0; i < 8; i++)
        {
            GameObject slot = new GameObject($"EquipmentSlot{i}");
            Image image = slot.AddComponent<Image>();
            if (slotTexture != null)
            {
                image.sprite = slotTexture;
            }
            image.color = new Color(1f, 1f, 1f, 0.95f);
            slot.transform.SetParent(equipmentSlots.transform, false);
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = item;
                UpdateSlot(i);
                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length) return;

        inventoryItems[slotIndex] = null;
        UpdateSlot(slotIndex);
    }

    private void UpdateSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length) return;

        Transform slotTransform = inventoryGrid.transform.GetChild(slotIndex);
        Image slotImage = slotTransform.GetComponent<Image>();

        if (inventoryItems[slotIndex] != null)
        {
            slotImage.sprite = inventoryItems[slotIndex].itemIcon;
            slotImage.color = Color.white;
        }
        else
        {
            slotImage.sprite = null;
            slotImage.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}