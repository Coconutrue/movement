using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class LoseSceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        YG2.onRewardAdv += RewardReceived;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= RewardReceived;
    }

    public void ClickShowRewardedButton()
    {
        YG2.RewardedAdvShow("respawn");
    }

    private void RewardReceived(string id)
    {
        if (id == "respawn")
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRespawnFromMenu();
            }
        }
    }

    public void ClickGoToMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
