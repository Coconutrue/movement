using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для перезапуска или смены сцен

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошел именно игрок
        // (убедитесь, что у вашего игрока стоит тег "Player")
        if (other.CompareTag("Player"))
        {
            Debug.Log("Уровень успешно завершен!");
            
            // Здесь пишется логика завершения игры. Например:
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene("Menu");
    }
}
