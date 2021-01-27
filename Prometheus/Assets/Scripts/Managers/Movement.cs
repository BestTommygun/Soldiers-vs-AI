using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    public GameObject character;
    private float turnSpeed = 80.0f;
    private float acceleration = 0.0f;
    private float accelerationMax = 0.01f;
    private float maxSpeed = 50.0f;

    private bool decelerating = false;
    void Update()
    {
        if (acceleration <= accelerationMax)
        {
            acceleration += 0.001f;
        }

        transform.Translate(0, -acceleration * Time.deltaTime, 0);
        if (acceleration < maxSpeed)
        {
            acceleration += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.left, turnSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime, Space.Self);

            //if (Input.GetKeyUp (KeyCode.Space)) {
            //rigidbody.velocity = rigidbody.velocity * 0.9;
        }
    }
}