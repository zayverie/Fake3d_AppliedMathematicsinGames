using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [Header("Health Settings")]
    public float currentHp;
    public float maxHp = 100f;

    [Header("Regen Settings")]
    public float regenRate = 1f;

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
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
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
        Vector3 playerDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            0f);

        transform.position += playerDirection.normalized * velocity * Time.fixedDeltaTime;

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

        if (transform.position.x >= 1f || transform.position.x <= -1f)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1f, 1f), transform.position.y, transform.position.z);
        }
    }
    public float ResetVerticalVelocity()
    {
        verticalVelocity = 0f;
        return verticalVelocity;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        if(UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHp, maxHp);
            UIManager.Instance.UpdateHealthText(currentHp, maxHp);
        }
        if (UIManager.Instance != null && UIManager.Instance.damageFlashPanel != null)
        {
            StartCoroutine(UIManager.Instance.FlashDamage());
        }
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
    
