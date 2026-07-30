using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere_rotate : MonoBehaviour
{
    [SerializeField] private Transform gateVisual;
    [SerializeField] private float speed = 90f;
    
    [SerializeField] private Vector3 rotateAxis = Vector3.up; 

    private bool shouldRotate = false;
    private float rotatedAmount = 0f;

    private void Update()
    {
        if (shouldRotate && gateVisual != null && rotatedAmount < 180f)
        {
            float rotationStep = speed * Time.deltaTime;
            if (rotatedAmount + rotationStep > 180f)
            {
                rotationStep = 180f - rotatedAmount;
            }
            gateVisual.Rotate(rotateAxis, rotationStep, Space.Self);
            rotatedAmount += rotationStep;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) shouldRotate = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) shouldRotate = false;
    }
}