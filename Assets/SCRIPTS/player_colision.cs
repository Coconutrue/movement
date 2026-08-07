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

        if (_movementScript != null) _movementScript.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_explosionPrefab != null)
        {
            Vector3 spawnPosition = _visual != null ? _visual.position : transform.position;
            Instantiate(_explosionPrefab, spawnPosition, Quaternion.identity);
        }

        if (_visual != null) _visual.gameObject.SetActive(false);

        Invoke(nameof(OpenMenuSceneAdditive), 1f);
    }

    private void OpenMenuSceneAdditive()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(_menuSceneName, LoadSceneMode.Additive);
    }

    public void Revive()
    {
        _isDead = false;
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(gameObject.scene);
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        yield return null;

        if (_visual != null) _visual.gameObject.SetActive(true);

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
        }

        if (_movementScript != null) _movementScript.enabled = true;

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
