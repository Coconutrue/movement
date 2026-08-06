using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Настройки сцен")]
    [Tooltip("Имя сцены главного меню")]
    [SerializeField] private string _mainMenuSceneName = "Menu";

    // Этот метод мы будем вызывать по нажатию на кнопку
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
