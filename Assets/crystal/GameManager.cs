using System.Collections;
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

    [Header("UI Времени")]
    public Text currentTimeText;
    public Text bestTimeText;

    private float _currentTime;
    private bool _isTimerRunning;
    private PlayerCollision _playerCollision;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            Time.timeScale = 1f; 
        }
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        if (playerObject != null)
        {
            _playerCollision = playerObject.GetComponent<PlayerCollision>();
        }

        YG2.onGetSDKData += LoadProgress;

        if (YG2.isSDKEnabled) 
        {
            LoadProgress();
        }
        else
        {
            UpdateUI();
        }

        _currentTime = 0f;
        _isTimerRunning = true;
    }

    private void OnDestroy()
    {
        YG2.onGetSDKData -= LoadProgress;
    }

    private void Update()
    {
        if (_isTimerRunning && Time.timeScale > 0f)
        {
            _currentTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void CompleteRespawnFromMenu()
    {
        StartCoroutine(UnloadMenuRoutine());
    }

    private IEnumerator UnloadMenuRoutine()
    {
        // ГАРАНТИРОВАННО ВКЛЮЧАЕМ ЗВУК: Снимаем блокировку аудиосистемы после рекламы Яндекс Игр
        AudioListener.pause = false; 

        // Снимаем паузу времени ДО выгрузки сцены, чтобы аудио-движок Unity мгновенно обновился
        Time.timeScale = 1f;

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("EscMenu");
        while (!unloadOp.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(gameObject.scene);

        if (_playerCollision != null)
        {
            _playerCollision.Revive();
            _isTimerRunning = true; 
        }
    }

    // Вызывать этот метод ровно в момент смерти игрока!
    public void StopTimerOnDeath()
    {
        _isTimerRunning = false;
        
        int finalSeconds = Mathf.FloorToInt(_currentTime);
        YG2.saves.lastTime = finalSeconds;

        if (finalSeconds > YG2.saves.bestTime)
        {
            YG2.saves.bestTime = finalSeconds;
        }

        // Важно: сохраняем результаты времени в облако Яндекса
        YG2.SaveProgress(); 
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        SaveProgress();
    }

    private void SaveProgress()
    {
        YG2.saves.money = totalScore;
        YG2.SaveProgress();
        UpdateUI();
    }

    public void LoadProgress()
    {
        totalScore = YG2.saves.money;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = " " + totalScore;

        if (bestTimeText != null)
            bestTimeText.text = " " + YG2.saves.bestTime ;
    }

    private void UpdateTimerUI()
    {
        if (currentTimeText != null)
            currentTimeText.text = " " + Mathf.FloorToInt(_currentTime);
    }
}
