using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Update()
    {
        if (Player.Instance != null)
        {
            if (Player.Instance.currentHp <= 0)
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.LoseGame();
                }
            }
        }
    } 
}
