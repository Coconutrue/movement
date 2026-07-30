using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_gravity : MonoBehaviour
{
    [SerializeField] private Rigidbody objectRigidbody;

    private void Start()
    {
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
            objectRigidbody.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objectRigidbody != null)
        {
            objectRigidbody.isKinematic = false;
            objectRigidbody.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
            objectRigidbody.useGravity = false;
        }
    }
}