using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCollision : MonoBehaviour
{
    public float Damage = 12;
    public Transform CollisionPrefab;
    private Drone_FDE fde;
    [ShowOnly] public Vector3 knockback;
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    private DroneTargetting droneTargetting;
    private void Start()
    {
        fde = transform.gameObject.GetComponent<Drone_FDE>();
        rigidbody = transform.gameObject.GetComponent<Rigidbody>();
        droneTargetting = transform.gameObject.GetComponent<DroneTargetting>();

        //initialize line renderer
        lineRenderer = transform.gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = transform.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        if (knockback.magnitude > 0)
        {
            transform.Translate(knockback * Time.deltaTime);
            knockback *= (1 - rigidbody.drag);
            DrawLine(transform.position, transform.position + knockback);
        }
    }
    public void DrawLine(Vector3 first, Vector3 second)
    {
        lineRenderer.SetPosition(0, first); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, second); //x,y and z position of the starting point of the line
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Indestructible object")
        {
            // Instantiate collision prop
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Instantiate(CollisionPrefab, position, rotation);

            // Change health
            HealthManager otherHealth = collision.gameObject.GetComponent<HealthManager>();
            if(otherHealth != null)
                otherHealth.ChangeHealth(-Damage);

            // Calculate knockback
            Vector3 newKnockback = transform.position - collision.transform.position;
            newKnockback = newKnockback.normalized;
            newKnockback *= 25;
            knockback = newKnockback;
        }
    }
}
