using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPersist : MonoBehaviour
{
    private static MusicPersist instance;

    [Header("Аудиоклипы для разных сцен")]
    public AudioClip menuMusic;   
    public AudioClip gameMusic;   

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudio();
            SwitchMusicForScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded; // Добавляем отслеживание закрытия сцен
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void SetupAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ЕСЛИ ОТКРЫЛОСЬ МЕНЮ СМЕРТИ — ставим игровую музыку на паузу
        if (scene.name == "EscMenu")
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause(); // Мягкая пауза (запоминает секунду трека)
            }
            return; 
        }

        AudioListener.pause = false;
        SwitchMusicForScene(scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        // ЕСЛИ МЕНЮ СМЕРТИ ЗАКРЫЛОСЬ (игрок возродился) — снимаем музыку с паузы
        if (scene.name == "EscMenu")
        {
            if (audioSource != null && !audioSource.isPlaying && audioSource.clip == gameMusic)
            {
                AudioListener.pause = false;
                audioSource.UnPause(); // Продолжает играть с того же места
            }
        }
    }

    void SwitchMusicForScene(string sceneName)
    {
        AudioClip targetClip = null;

        if (sceneName == "Menu")
        {
            targetClip = menuMusic;
        }
        else if (sceneName == "SampleScene")
        {
            targetClip = gameMusic;
        }

        if (targetClip != null)
        {
            if (audioSource.clip == targetClip)
            {
                // Если этот трек уже стоял на паузе, просто снимаем с паузы
                if (!audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
                return;
            }

            audioSource.Stop();
            audioSource.clip = targetClip;
            AudioListener.pause = false;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}
