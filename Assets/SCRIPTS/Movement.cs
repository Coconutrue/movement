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

    [Header("Настройки Свайпа / Сенсора")]
    [Tooltip("Максимальная дистанция свайпа в пикселях для достижения полной скорости")]
    [SerializeField] private float _swipeMaxDistance = 150f;
    [Tooltip("Чувствительность сенсора (множитель скорости)")]
    [SerializeField] private float _touchSensitivity = 1.2f;

    [Header("Ссылки")]
    [Tooltip("Перетащите сюда дочернюю 3D-модель самолёта")]
    [SerializeField] private Transform _visualModel; 
    
    private float _currentStrafeSpeed = 0f;
    private float _currentTilt = 0f;
    private Quaternion _initialVisualRotation;
    private Transform _transform;

    // Переменные для расчета свайпа
    private Vector2 _touchStartPos;
    private float _touchInputX = 0f;
    private float _lastTouchX; 
    
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
        
        HandleMobileTouch(); 
        MoveForward(deltaTime);
        MoveStrafe(deltaTime);
    }

    private void MoveForward(float deltaTime)
    {
        _transform.Translate(_forwardSpeed * deltaTime * Vector3.right);
    }

    private void HandleMobileTouch()
    {
        if (Input.touchCount == 0)
        {
            _touchInputX = 0f;
            return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _touchStartPos = touch.position;
                _lastTouchX = touch.position.x;
                _touchInputX = 0f;
                break;

            case TouchPhase.Moved:
                float currentX = touch.position.x;
                
                // Мгновенный сброс инерции при смене знака движения пальца
                if ((currentX > _lastTouchX && _touchInputX < 0f) || (currentX < _lastTouchX && _touchInputX > 0f))
                {
                    _touchStartPos = touch.position;
                    _touchInputX = 0f;
                    _currentStrafeSpeed = 0f; // Сбрасываем физическую скорость в ноль для мгновенного отклика
                }

                _lastTouchX = currentX;

                float deltaX = currentX - _touchStartPos.x;
                _touchInputX = Mathf.Clamp(deltaX / _swipeMaxDistance, -1f, 1f) * _touchSensitivity;
                break;

            case TouchPhase.Stationary:
                _lastTouchX = touch.position.x;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                _touchInputX = 0f;
                break;
        }
    }

    private void MoveStrafe(float deltaTime)
    {
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

        // Целевой ввод (мгновенный, без задержек)
        float targetInput = Mathf.Abs(_touchInputX) > 0.01f ? _touchInputX : keyboardInput;
        
        // Инверсия
        float finalInput = -targetInput; 

        // Расчет целевой скорости, к которой самолет должен стремиться
        float targetSpeed = finalInput * _strafeSpeed;

        // ПЛАВНОСТЬ ТУТ: Самолет плавно разгоняется до целевой скорости. Не дергается, но слушается сразу!
        _currentStrafeSpeed = Mathf.Lerp(_currentStrafeSpeed, targetSpeed, deltaTime * _movementSmoothness);

        // Движение на основе сглаженной скорости
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
}
