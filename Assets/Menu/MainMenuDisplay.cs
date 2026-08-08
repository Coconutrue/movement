using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuDisplay : MonoBehaviour
{
    // Статическая ссылка, чтобы другие скрипты могли обращаться к этому меню
    public static MainMenuDisplay Instance { get; private set; }

    [Header("Текстовые поля UI")]
    [SerializeField] private Text _bestTimeText;
    [SerializeField] private Text _lastTimeText;
    [SerializeField] private Text _scoreText; 

    private void Awake()
    {
        // Инициализируем синглтон
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += DisplayStats;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= DisplayStats;
    }

    private void Start()
    {
        if (YG2.isSDKEnabled)
        {
            DisplayStats();
        }
    }

    public void DisplayStats()
    {
        if (_bestTimeText != null)
        {
            _bestTimeText.text = " " + YG2.saves.bestTime;
        }

        if (_lastTimeText != null)
        {
            _lastTimeText.text = " " + YG2.saves.lastTime;
        }

        if (_scoreText != null)
        {
            _scoreText.text = " " + YG2.saves.money;
        }
    }
}
