using UnityEngine;

public class CustomFloatandFall : MonoBehaviour
{
    [Header("Настройки высоты")]
    public float targetHeight = 3f;      // Высота подъема от начальной точки

    [Header("Настройки скорости")]
    public float upSpeed = 1f;           // Скорость плавного подъема
    public float downSpeed = 15f;        // Скорость резкого падения

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isRising = true;        // Флаг направления движения

    void Start()
    {
        startPos = transform.position;
        // Вычисляем верхнюю точку относительно старта
        targetPos = startPos + Vector3.up * targetHeight; 
    }

    void Update()
    {
        if (isRising)
        {
            // Плавно движемся вверх к targetPos
            transform.position = Vector3.MoveTowards(transform.position, targetPos, upSpeed * Time.deltaTime);

            // Если достигли верхней точки, переключаемся на падение
            if (transform.position == targetPos)
            {
                isRising = false;
            }
        }
        else
        {
            // Резко движемся вниз к startPos
            transform.position = Vector3.MoveTowards(transform.position, startPos, downSpeed * Time.deltaTime);

            // Если упали в начальную точку, снова начинаем подъем
            if (transform.position == startPos)
            {
                isRising = true;
            }
        }
    }
}
