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
            // Снимаем блокировку звука сразу после закрытия рекламного окна Яндекса
            AudioListener.pause = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRespawnFromMenu();
            }
        }
    }

    public void ClickGoToMainMenuButton()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false; // ГАРАНТИРУЕТ, что звук в Unity снова включится
        SceneManager.LoadScene("Menu"); // Проверьте, что в скрипте музыки имя тоже "Menu"!
    }
}
