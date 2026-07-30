using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; 

    [SerializeField] private Vector3 _offset; 
    [SerializeField] private float _smoothSpeed = 5f;

    private void Start()
    {
        if (_offset == Vector3.zero && _target != null)
        {
            _offset = transform.position - _target.position;
        }
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
    }
}