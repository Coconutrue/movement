using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_right : MonoBehaviour

{
    public float speed = 5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.linearVelocity; 
        currentVelocity.z = speed;
        rb.linearVelocity = currentVelocity;
    }

}
