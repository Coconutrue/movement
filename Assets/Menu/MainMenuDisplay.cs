using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuDisplay : MonoBehaviour
{
    [Header("Текстовые поля UI")]
    [SerializeField] private Text _bestTimeText;
    [SerializeField] private Text _lastTimeText;
    [SerializeField] private Text _scoreText; // Добавили поле для кристаллов/монет

    private void OnEnable()
    {
        // Подписываемся на событие загрузки данных из облака Яндекса
        YG2.onGetSDKData += DisplayStats;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= DisplayStats;
    }

    private void Start()
    {
        // Если SDK уже готов (например, при повторном выходе в меню), сразу обновляем UI
        if (YG2.isSDKEnabled)
        {
            DisplayStats();
        }
    }

    public void DisplayStats()
    {
        // Отображение лучшего времени
        if (_bestTimeText != null)
        {
            _bestTimeText.text = " " + YG2.saves.bestTime;
        }

        // Отображение последнего забега
        if (_lastTimeText != null)
        {
            _lastTimeText.text = " " + YG2.saves.lastTime;
        }

        // Отображение кристаллов/денег в меню
        if (_scoreText != null)
        {
            _scoreText.text = " " + YG2.saves.money;
        }
    }
}
