using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player Instance => instance;
    private static Player instance;
    private float velocity = 5f;
    private float jumpForce = 5f;
    private float gravity = 9.81f;
    public float verticalVelocity {get; private set;}
    public bool isGrounded;
    public bool isCollidingWithItem;
    public bool isCollidingWithObstacle;
    public bool _jumpPressed;

    [Header("Lane Movement")]
    private float[] lanes = new float[] { -1f, 0f, 1f };
    private int currentLaneIndex = 1; 
    [SerializeField] private float laneShiftSpeed = 20f; 
    
    [Header("Health Settings")]
    public float currentHp;
    public float maxHp = 100f;

    [Header("Regen Settings")]
    public float regenRate = 1f;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Invincibility Settings")]
    private float invincibilityDuration = .5f;
    public bool isInvincible { get; private set; } = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }
    void Start()
    {
        currentHp = maxHp;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHp, maxHp);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthText(currentHp, maxHp);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentLaneIndex > 0)
            {
                currentLaneIndex--;
                if (anim != null)
                {
                    anim.SetTrigger("Left");
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentLaneIndex < lanes.Length - 1)
            {
                currentLaneIndex++;
                if (anim != null)
                {
                    anim.SetTrigger("Right");
                }
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _jumpPressed = true;
        }

        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
        }

        if (currentHp <= 0)
        {
            UIManager.Instance.LoseGame();
            Debug.Log("Player has been defeated!");
        }

        if (currentHp < maxHp)
        {
            RegenerateHealth();
        }
    }

    void FixedUpdate()
    {  
        float targetX = lanes[currentLaneIndex];
        float moveX = Mathf.MoveTowards(transform.position.x, targetX, laneShiftSpeed * Time.fixedDeltaTime);
        transform.position = new Vector3(moveX, transform.position.y, transform.position.z);

        if (_jumpPressed && isGrounded)
        {
            Debug.Log("Jump pressed");
            verticalVelocity = jumpForce;
            isGrounded = false;
            Player.Instance.isCollidingWithItem = false;
        }
        _jumpPressed = false;
        verticalVelocity -= gravity * Time.fixedDeltaTime;
        transform.position += new Vector3(0f, verticalVelocity * Time.fixedDeltaTime, 0f);

        float groundY = -3f;
        if (transform.position.y < groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            verticalVelocity = 0f;
            isGrounded = true;
        }
    }
    public float ResetVerticalVelocity()
    {
        verticalVelocity = 0f;
        return verticalVelocity;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvincible)
        {
            return;
        }

        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHp, maxHp);
            UIManager.Instance.UpdateHealthText(currentHp, maxHp);
        }
        if (UIManager.Instance != null && UIManager.Instance.damageFlashPanel != null)
        {
            StartCoroutine(UIManager.Instance.FlashDamage());
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        if (anim != null)
        {
            anim.SetBool("isHit", true);
        }

        yield return new WaitForSeconds(invincibilityDuration);

        if (anim != null)
        {
            anim.SetBool("isHit", false);
        }

        isInvincible = false;
    }

    public void RegenerateHealth()
    {
        if (currentHp < maxHp)
        {
            currentHp += regenRate * Time.fixedDeltaTime;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthBarRegen(currentHp, maxHp);
                UIManager.Instance.UpdateHealthText(currentHp, maxHp);
            }
        }
    }
}
    
