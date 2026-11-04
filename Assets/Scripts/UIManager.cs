using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance => instance;
    private static UIManager instance;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject damageFlashPanel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        if(gameOverPanel != null && gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(false);
        }
        if(gameWinPanel != null && gameWinPanel.activeSelf)
        {
            gameWinPanel.SetActive(false);
        }
        if(damageFlashPanel != null && damageFlashPanel.activeSelf)
        {
            damageFlashPanel.SetActive(false);
        }
    }

    public System.Collections.IEnumerator FlashDamage()
    {
        if (damageFlashPanel != null)
        {
            damageFlashPanel.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            damageFlashPanel.SetActive(false);
        }
    }

}
