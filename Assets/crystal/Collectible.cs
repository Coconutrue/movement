using UnityEngine;
using YG; // Если используете PluginYG для Яндекс Игр

public class Collectible : MonoBehaviour
{
    public int scoreValue = 1; // Сколько очков дает объект
    [Header("Настройки звука")]
    public AudioClip collectionSound; // Сюда перетащим аудиофайл
    
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект еще не собран и коснулся именно Player
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;
            
            // 1. Добавляем очки (убедитесь, что GameManager существует в проекте)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            // 2. Воспроизводим звук в точке сбора
            if (collectionSound != null)
            {
                AudioSource.PlayClipAtPoint(collectionSound, transform.position);
            }

            // 3. Уничтожаем кристалл
            Destroy(gameObject);
        }
    }
}
