using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int totalScore;
    public Text scoreText; // Ссылка на UI текст счета

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
{
    if (YG2.isSDKEnabled) 
    {
        LoadProgress();
    }
    
    UpdateUI(); 
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

    // Этот метод мы теперь будем вызывать автоматически при загрузке плагина
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
