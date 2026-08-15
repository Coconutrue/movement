using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройка стороны (-1 = Лево, 1 = Право)")]
    [SerializeField] private float _inputDirection = 0f;

    // Статическое свойство, которое сможет читать скрипт движения
    public static float TouchHorizontal { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Когда палец коснулся кнопки, задаем направление движения
        TouchHorizontal = _inputDirection;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Когда палец убрали, сбрасываем ввод в 0 (только если значение совпадает)
        if (TouchHorizontal == _inputDirection)
        {
            TouchHorizontal = 0f;
        }
    }

    private void OnDisable()
    {
        TouchHorizontal = 0f;
    }
}
