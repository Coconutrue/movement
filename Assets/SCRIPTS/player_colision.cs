using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerCollision : MonoBehaviour
{
    [Header("Эффект взрыва")]
    [Tooltip("Перетащите сюда Prefab взрыва (частицы или анимацию)")]
    [SerializeField] private GameObject _explosionPrefab;

    private Movement _movementScript;
    private bool _isDead = false;

    private void Start()
    {
        _movementScript = GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other)
{
    if (_isDead) return;

    if (other.CompareTag("Obstacle"))
    {
        Explode();
    }
}
    private void Explode()
    {
        _isDead = true;

        if (_movementScript != null)
        {
            _movementScript.enabled = false;
        }

        Transform visual = transform.Find("Plane_Mesh") ?? transform.GetChild(0);

        if (_explosionPrefab != null)
        {
            Vector3 spawnPosition = visual != null ? visual.position : transform.position;

            Instantiate(_explosionPrefab, spawnPosition, Quaternion.identity);
        }

        if (visual != null)
        {
            visual.gameObject.SetActive(false);
        }

        Invoke(nameof(RestartGame), 2f);
    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}