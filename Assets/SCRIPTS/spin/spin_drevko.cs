using System.Collections;
using UnityEngine;

public class SmoothRotateOnTrigger : MonoBehaviour
{
    public Transform targetObject; 
    public Vector3 rotationAxis = Vector3.up; 
    public float angle = 40f; 
    public float duration = 1.0f; // Время плавного поворота в секундах

    private bool hasRotated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasRotated && other.CompareTag("Player"))
        {
            if (targetObject != null)
            {
                StartCoroutine(SmoothRotate());
                hasRotated = true;
            }
        }
    }

    private IEnumerator SmoothRotate()
    {
        Quaternion startRotation = targetObject.rotation;
        // Вычисляем конечный поворот относительно текущего
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, rotationAxis);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Плавно интерполируем между начальным и конечным поворотом
            targetObject.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            yield return null; 
        }

        // Фиксируем точный финальный угол
        targetObject.rotation = endRotation;
    }
}
