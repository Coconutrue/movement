using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);

    [SerializeField] private float _forwardSpeed = 15f;
    [SerializeField] private float _strafeSpeed = 7f;

    [SerializeField] private float _maxTiltAngle = 35f; 
    [SerializeField] private float _tiltSpeed = 4f;     

    [Tooltip("Перетащите сюда дочернюю 3D-модель самолёта")]
    [SerializeField] private Transform _visualModel; 
    private float _smoothSideInput = 0f;
    private float _currentTilt = 0f;
    private Quaternion _initialVisualRotation;

    private void Start()
    {
        if (_visualModel == null && transform.childCount > 0)
        {
            _visualModel = transform.GetChild(0);
        }

        if (_visualModel != null)
        {
            _initialVisualRotation = _visualModel.localRotation;
        }
    }

    private void Update()
    {
        MoveForward();
        MoveStrafe();
    }

    private void MoveForward()
    {
        float forwardDistance = _forwardSpeed * Time.deltaTime;
        transform.Translate(forwardDistance * Vector3.right);
    }

    private void MoveStrafe()
    {
        // 1. Получаем ввод с клавиатуры и сенсора
        float keyboardInput = Input.GetAxis(Horizontal);
        float touchInput = MobileInput.TouchHorizontal;

        // 2. Выбираем целевое направление
        float targetInput = Mathf.Abs(touchInput) > 0.1f ? touchInput : keyboardInput;

        // 3. ПЛАВНОСТЬ: Эмулируем поведение клавиатуры для сенсора.
        // Меняйте цифру 5f (скорость изменения), чтобы сделать управление отзывчивее или плавнее.
        _smoothSideInput = Mathf.MoveTowards(_smoothSideInput, targetInput, Time.deltaTime * 5f);

        // 4. Применяем вашу оригинальную инверсию осей
        float finalInput = -_smoothSideInput; 

        // 5. Движение по вашей кастомной оси Vector3.forward
        float strafeDistance = finalInput * _strafeSpeed * Time.deltaTime;
        transform.Translate(strafeDistance * Vector3.forward);

        // Наклон модели на основе сглаженного ввода
        Tilt(finalInput);
    }

    private void Tilt(float inputX)
    {
        if (_visualModel == null) return;

        float targetTilt = inputX * _maxTiltAngle;
        _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, Time.deltaTime * _tiltSpeed);

        _visualModel.localRotation = _initialVisualRotation * Quaternion.Euler(_currentTilt, 0f, 0f);
    }
}
