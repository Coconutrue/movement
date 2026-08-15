using UnityEngine;

public class Movement : MonoBehaviour
{
    // Заменяем GetAxis на GetAxisRaw (работает быстрее, без встроенного сглаживания Unity, так как мы пишем свое)
    private const string HorizontalAxis = "Horizontal"; 

    [SerializeField] private float _forwardSpeed = 15f;
    [SerializeField] private float _strafeSpeed = 7f;

    [SerializeField] private float _maxTiltAngle = 35f; 
    [SerializeField] private float _tiltSpeed = 4f;     

    [Tooltip("Перетащите сюда дочернюю 3D-модель самолёта")]
    [SerializeField] private Transform _visualModel; 
    private float _smoothSideInput = 0f;
    private float _currentTilt = 0f;
    private Quaternion _initialVisualRotation;
    
    // Кэшируем transform, так как частые обращения к свойству в старых версиях Unity тяжелее прямых ссылок
    private Transform _transform;

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
        float deltaTime = Time.deltaTime; // Кэшируем deltaTime для небольшого ускорения в рамках кадра
        
        MoveForward(deltaTime);
        MoveStrafe(deltaTime);
    }

    private void MoveForward(float deltaTime)
    {
        float forwardDistance = _forwardSpeed * deltaTime;
        _transform.Translate(forwardDistance * Vector3.right);
    }

    private void MoveStrafe(float deltaTime)
    {
        // Используем GetAxisRaw вместо GetAxis для производительности
        float keyboardInput = Input.GetAxisRaw(HorizontalAxis);
        float touchInput = MobileInput.TouchHorizontal;

        // Быстрая проверка через тернарный оператор
        float targetInput = Mathf.Abs(touchInput) > 0.1f ? touchInput : keyboardInput;

        // Плавность сглаживания
        _smoothSideInput = Mathf.MoveTowards(_smoothSideInput, targetInput, deltaTime * 5f);

        float finalInput = -_smoothSideInput; 

        float strafeDistance = finalInput * _strafeSpeed * deltaTime;
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
