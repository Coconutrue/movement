using UnityEngine;
using UnityEngine.UI; // Используйте using TMPro; если у вас TextMeshPro
using YG;

public class MainMenuUI : MonoBehaviour
{
    public Text crystalText; // Если TextMeshPro, замените на: public TMP_Text crystalText;

    private void Start()
    {
        // Если плагин уже готов к моменту старта сцены меню, сразу выводим счет
        if (YG2.isSDKEnabled)
        {
            UpdateMenuUI();
        }
    }

    // Этот метод мы теперь будем вызывать автоматически при загрузке плагина в меню
    public void UpdateMenuUI()
    {
        if (crystalText != null)
        {
            // Напрямую берем сохраненное количество монет из Яндекса
            crystalText.text = "Кристаллы: " + YG2.saves.money;
        }
    }
}
