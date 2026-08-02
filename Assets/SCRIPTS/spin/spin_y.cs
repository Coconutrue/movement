using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin_y : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}