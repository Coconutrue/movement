using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int totalScore;
    public Text scoreText;
    public GameObject playerObject;

    private PlayerCollision _playerCollision;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        if (playerObject != null)
        {
            _playerCollision = playerObject.GetComponent<PlayerCollision>();
        }

        if (YG2.isSDKEnabled) LoadProgress();
        UpdateUI();
    }

    public void CompleteRespawnFromMenu()
    {
        SceneManager.UnloadSceneAsync("EscMenu");
        if (_playerCollision != null)
        {
            _playerCollision.Revive();
        }
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        UpdateUI();
        SaveProgress();
    }

    private void SaveProgress()
    {
        YG2.saves.money = totalScore;
        YG2.SaveProgress();
    }

    public void LoadProgress()
    {
        totalScore = YG2.saves.money;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Счет: " + totalScore;
    }
}
