using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance => instance;
    private static UIManager instance;
    public GameObject gameplayUI;
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject damageFlashPanel;

    [Header("Health Bar Fill")]
    public Image healthFillImage;
    public Image ghostHpFillImage;
    public TextMeshProUGUI healthText;

    [Header("Ghost Bar")]
    [SerializeField] private float ghostHpFillDuration = 0.6f; 
    [SerializeField] private float ghostHpDelay = 0.2f;

    private float startGhostFill = 1f;
    private float targetFill = 1f;
    private float elapsedTime = 0f;
    private float delayTimer = 0f;

    private void Awake()
    {
        Time.timeScale = 1f;
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        if (losePanel != null && losePanel.activeSelf)
        {
            losePanel.SetActive(false);
        }

        if (winPanel != null && winPanel.activeSelf)
        {
            winPanel.SetActive(false);
        }

        if (damageFlashPanel != null && damageFlashPanel.activeSelf)
        {
            damageFlashPanel.SetActive(false);
        }
    }

    public void Update()
    {
        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
        }
        else if (ghostHpFillImage != null && elapsedTime < ghostHpFillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ghostHpFillDuration); 

            float easedT = Ease.EaseOutCubic(t);

            ghostHpFillImage.fillAmount = startGhostFill + (targetFill - startGhostFill) * easedT;
        }

    }

    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        targetFill = currentHp / maxHp;
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(currentHp / maxHp);
        }

        if (ghostHpFillImage != null)
        {
            startGhostFill = ghostHpFillImage.fillAmount;
            elapsedTime = 0f;
            delayTimer = ghostHpDelay; 
        }
    }

    public void UpdateHealthText(float currentHp, float maxHp)
    {
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(Mathf.Max(0, currentHp))} / {Mathf.CeilToInt(maxHp)}";
        }
    }

    public void UpdateHealthBarRegen(float currentHp, float maxHp)
    {
        targetFill = currentHp / maxHp;

        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(targetFill);
        }

        if (ghostHpFillImage != null && ghostHpFillImage.fillAmount < healthFillImage.fillAmount)
        {
            ghostHpFillImage.fillAmount = healthFillImage.fillAmount;
        }
    }

    public void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void LoseGame()
    {
        if (losePanel != null)
        {
            gameplayUI.SetActive(false);
            losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public IEnumerator FlashDamage()
    {
        if (damageFlashPanel != null)
        {
            damageFlashPanel.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            damageFlashPanel.SetActive(false);
        }
    }
}
