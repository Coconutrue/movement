using UnityEngine;
using UnityEngine.SceneManagement; // Обязательно для работы со сценами

public class MusicSample : MonoBehaviour
{
    private static MusicSample instance;

    [Header("Настройки аудио")]
    public AudioClip menuMusic;
    [Tooltip("Точное название сцены, где музыка ДОЛЖНА играть")]
    public string menuSceneName = "SampleScene"; 

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Подписываемся на событие смены сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Отписываемся от события при удалении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SetupAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
        }
    }

    // Этот метод вызывается автоматически каждый раз, когда загружается ЛЮБАЯ сцена
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == menuSceneName)
        {
            // Если мы вернулись в меню — включаем музыку (если она не играет)
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Если загрузилась любая другая сцена — полностью выключаем звук
            audioSource.Stop();
        }
    }
}
