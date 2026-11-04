using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool hasLost = false;

    public void Update()
    {   
        if (!hasLost && Player.Instance != null && Player.Instance.currentHp <= 0)
        {
            hasLost = true;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.LoseGame();
            }
        }
    } 
}
