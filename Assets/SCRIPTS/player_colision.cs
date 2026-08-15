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
    private bool _isDead = false;
    private bool _isInvulnerable = false;
    private Transform _visual;
    private int _respawnCount = 0; 

    private void Start()
    {
        _movementScript = GetComponent<Movement>();
        _visual = transform.Find("Plane_Mesh") ?? transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead || _isInvulnerable) return;

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

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_explosionPrefab != null)
        {
            Vector3 spawnPosition = _visual != null ? _visual.position : transform.position;
            Instantiate(_explosionPrefab, spawnPosition, Quaternion.identity);
        }

        if (_visual != null) _visual.gameObject.SetActive(false);

        if (_respawnCount < 2)
        {
            Invoke(nameof(OpenMenuSceneAdditive), 1f);
        }
        else
        {
            Invoke(nameof(GameOverToMainMenu), 1f);
        }
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

        var eventSystems = FindObjectsByType<UnityEngine.EventSystems.EventSystem>(FindObjectsSortMode.None);
        foreach (var es in eventSystems)
        {
            if (es.gameObject.scene != gameObject.scene)
            {
                Destroy(es.gameObject);
            }
        }

        if (_visual != null) _visual.gameObject.SetActive(true);

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
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
                _visual.gameObject.SetActive(!_visual.gameObject.activeSelf);
            }
            yield return new WaitForSecondsRealtime(_blinkInterval);
            timer += _blinkInterval;
        }

        if (_visual != null) _visual.gameObject.SetActive(true);
        _isInvulnerable = false;
    }
}
