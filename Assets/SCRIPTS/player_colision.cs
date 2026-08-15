using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private string _menuSceneName = "EscMenu";
    [SerializeField] private float _invulnerabilityDuration = 2f;
    [SerializeField] private float _blinkInterval = 0.2f;

    private Movement _movementScript;
    private Rigidbody _rigidbody; // Кэшируем Rigidbody сразу
    private bool _isDead = false;
    private bool _isInvulnerable = false;
    private Transform _visual;
    private int _respawnCount = 0; 
    
    // Оптимизация памяти: кэшируем объект ожидания для корутины
    private WaitForSecondsRealtime _blinkWait; 

    private void Start()
    {
        _movementScript = GetComponent<Movement>();
        _rigidbody = GetComponent<Rigidbody>();
        _visual = transform.Find("Plane_Mesh") ?? transform.GetChild(0);
        
        // Создаем объект ожидания один раз при старте, чтобы избежать создания мусора в памяти (GC)
        _blinkWait = new WaitForSecondsRealtime(_blinkInterval);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead || _isInvulnerable) return;

        // Сравнение тегов через CompareTag — это уже оптимизированный вариант, оставляем его
        if (other.CompareTag("Obstacle"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        _isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StopTimerOnDeath();
        }
        
        if (_movementScript != null) _movementScript.enabled = false;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        if (_explosionPrefab != null)
        {
            Vector3 spawnPosition = _visual != null ? _visual.position : transform.position;
            // СОВЕТ: Замените Instantiate на вызов из вашего Object Pool (например: PoolManager.Spawn(_explosionPrefab, ...))
            Instantiate(_explosionPrefab, spawnPosition, Quaternion.identity);
        }

        if (_visual != null) _visual.gameObject.SetActive(false);

        // Замена Invoke на более производительную проверку времени или простую корутину, 
        // но для редких одиночных вызовов при смерти Invoke допустим. На мобилках лучше запускать Coroutine.
        if (_respawnCount < 2)
        {
            StartCoroutine(WaitAndExecute(1f, OpenMenuSceneAdditive));
        }
        else
        {
            StartCoroutine(WaitAndExecute(1f, GameOverToMainMenu));
        }
    }

    private IEnumerator WaitAndExecute(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    private void OpenMenuSceneAdditive()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Additive);
    }

    private void GameOverToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); 
    }

    public void Revive()
    {
        _isDead = false;
        _respawnCount++; 
        Time.timeScale = 1f;

        // ВНИМАНИЕ: FindObjectsByType — это критический удар по FPS на мобилках!
        // Рекомендуется убрать этот блок. Настройте сцену EscMenu так, чтобы на ней просто НЕ БЫЛО своего EventSystem,
        // либо отключайте его скриптом в самой сцене меню, не сканируя всю основную игру.
        /*
        var eventSystems = FindObjectsByType<UnityEngine.EventSystems.EventSystem>(FindObjectsSortMode.None);
        foreach (var es in eventSystems)
        {
            if (es.gameObject.scene != gameObject.scene)
            {
                Destroy(es.gameObject);
            }
        }
        */

        if (_visual != null) _visual.gameObject.SetActive(true);

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.WakeUp();
        }

        if (_movementScript != null)
        {
            _movementScript.enabled = true;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        _isInvulnerable = true;
        float timer = 0f;

        while (timer < _invulnerabilityDuration)
        {
            if (_visual != null)
            {
                // Быстрое переключение активности без лишних проверок
                _visual.gameObject.SetActive(!_visual.gameObject.activeSelf);
            }
            // Используем закэшированный WaitForSecondsRealtime, мусор (GC) больше не генерируется!
            yield return _blinkWait; 
            timer += _blinkInterval;
        }

        if (_visual != null) _visual.gameObject.SetActive(true);
        _isInvulnerable = false;
    }
}
