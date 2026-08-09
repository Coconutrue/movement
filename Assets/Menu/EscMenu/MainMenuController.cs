using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Настройки сцен")]
    [Tooltip("Имя сцены главного меню")]
    [SerializeField] private string _mainMenuSceneName = "Menu";

    public void GoToMainMenu()
    {
        // ВОТ ЗДЕСЬ ВОЗВРАЩАЕМ ВРЕМЯ В НОРМУ:
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
