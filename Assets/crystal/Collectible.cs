using UnityEngine;
using YG; // Если используете PluginYG для Яндекс Игр

public class Collectible : MonoBehaviour
{
    public int scoreValue = 1; // Сколько очков дает объект
    private AudioSource audioSource;
    private bool isCollected = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;
            
            // 1. Добавляем очки в общий менеджер или статичную переменную
            GameManager.Instance.AddScore(scoreValue);

            // 2. Издаем звук (проигрываем в точку объекта, чтобы звук не обрывался при Destroy)
            if (audioSource != null && audioSource.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

            // 3. Уничтожаем объект
            Destroy(gameObject);
        }
    }
}
