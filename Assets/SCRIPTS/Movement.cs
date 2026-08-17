using UnityEngine;

public class Movement : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal"; 

    [Header("Настройки скорости")]
    [SerializeField] private float _forwardSpeed = 15f;
    [SerializeField] private float _strafeSpeed = 7f;
    [Tooltip("Плавность разгона и торможения самолета (чем меньше цифра, тем плавнее скользит)")]
    [SerializeField] private float _movementSmoothness = 8f;

    [Header("Настройки наклона")]
    [SerializeField] private float _maxTiltAngle = 35f; 
    [SerializeField] private float _tiltSpeed = 6f;     

    [Header("Ссылки")]
    [Tooltip("Перетащите сюда дочернюю 3D-модель самолёта")]
    [SerializeField] private Transform _visualModel; 
    
    private float _currentStrafeSpeed = 0f;
    private float _currentTilt = 0f;
    private Quaternion _initialVisualRotation;
    private Transform _transform;

    // Переменные для кнопочного ввода
    private float _uiInputX = 0f;
    private float _lastUiInputX = 0f;
    private float _lastKeyboardInput = 0f;

    private void Start()
    {
        _transform = transform;

        if (_visualModel == null && _transform.childCount > 0)
        {
            _visualModel = _transform.GetChild(0);
        }

        if (_visualModel != null)
        {
            _initialVisualRotation = _visualModel.localRotation;
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        
        MoveForward(deltaTime);
        MoveStrafe(deltaTime);
    }

    private void MoveForward(float deltaTime)
    {
        _transform.Translate(_forwardSpeed * deltaTime * Vector3.right);
    }

    private void MoveStrafe(float deltaTime)
    {
        // 1. Считываем клавиатуру (для тестов в редакторе)
        float keyboardInput = Input.GetAxisRaw(HorizontalAxis);

        // Мгновенный сброс физической скорости при смене кнопок A/D
        if ((keyboardInput > 0f && _lastKeyboardInput < 0f) || (keyboardInput < 0f && _lastKeyboardInput > 0f))
        {
            _currentStrafeSpeed = 0f; 
        }
        if (Mathf.Abs(keyboardInput) > 0.01f)
        {
            _lastKeyboardInput = keyboardInput;
        }

        // 2. Определяем итоговый ввод (приоритет у UI-кнопок, если они зажаты)
        float targetInput = Mathf.Abs(_uiInputX) > 0.01f ? _uiInputX : keyboardInput;
        
        // Инверсия (сохраняем вашу исходную логику направления)
        float finalInput = -targetInput; 

        // Расчет целевой скорости
        float targetSpeed = finalInput * _strafeSpeed;

        // Плавный разгон и торможение через Lerp
        _currentStrafeSpeed = Mathf.Lerp(_currentStrafeSpeed, targetSpeed, deltaTime * _movementSmoothness);

        // Движение самолета
        float strafeDistance = _currentStrafeSpeed * deltaTime;
        _transform.Translate(strafeDistance * Vector3.forward);

        Tilt(finalInput, deltaTime);
    }

    private void Tilt(float inputX, float deltaTime)
    {
        if (_visualModel == null) return;

        float targetTilt = inputX * _maxTiltAngle;
        _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, deltaTime * _tiltSpeed);

        _visualModel.localRotation = _initialVisualRotation * Quaternion.Euler(_currentTilt, 0f, 0f);
    }

    /// <summary>
    /// Метод для вызова из UI-кнопок (устанавливает направление движения)
    /// </summary>
    /// <param name="value">-1 для влево, 1 для вправо, 0 если отпустили</param>
    public void SetMobileInput(float value)
    {
        // УДАЛЕНО: Больше не сбрасываем _currentStrafeSpeed в 0f!
        // Теперь самолет будет плавно гасить скорость влево и разгоняться вправо.

        _uiInputX = value;

        if (Mathf.Abs(value) > 0.01f)
        {
            _lastUiInputX = value;
        }
    }
}
