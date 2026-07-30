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
        float sideInput = Input.GetAxis(Horizontal);
        sideInput = -sideInput; 

        float strafeDistance = sideInput * _strafeSpeed * Time.deltaTime;

        transform.Translate(strafeDistance * Vector3.forward);

        Tilt(sideInput);
    }

    private void Tilt(float inputX)
    {
        if (_visualModel == null) return;

        float targetTilt = inputX * _maxTiltAngle;
        _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, Time.deltaTime * _tiltSpeed);

        _visualModel.localRotation = _initialVisualRotation * Quaternion.Euler(_currentTilt, 0f, 0f);
    }
}
