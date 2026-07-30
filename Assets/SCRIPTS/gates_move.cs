using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gates_move : MonoBehaviour
{
    [SerializeField] private Transform gateVisual; 
    [SerializeField] private float speed = 0.4f;
    private Vector3 moveDirection = new Vector3(0f, 0f, 1f);
    private bool shouldMove = false;

    private void Start()
    {
        moveDirection.Normalize();
    }

    private void Update()
    {
        if (shouldMove && gateVisual != null)
        {
            gateVisual.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) shouldMove = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) shouldMove = false;
    }
}
