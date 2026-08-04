using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButtonID : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText; // Ссылка на текст внутри кнопки
    private int myIndex;
    private System.Action<int> onCLickCallback;

    // Метод инициализации кнопки из главного скрипта
    public void SetupButton(int index, string name, System.Action<int> clickAction)
    {
        myIndex = index;
        buttonText.text = name;
        onCLickCallback = clickAction;

        // Вешаем событие клика
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ButtonPress);
    }

    private void ButtonPress()
    {
        // Передаем сигнал главному менеджеру
        onCLickCallback?.Invoke(myIndex);
    }
}
