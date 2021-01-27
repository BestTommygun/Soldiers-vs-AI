using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_FDE : MonoBehaviour
{
    public GameObject target;
    private bool hasTarget;
    [ShowOnly]
    public Vector3 force;
    public float Drag = 0.10f;
    public float maxAcceleration = 50.0f;
    private Vector3 dragVector;
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    #region Attractions and repulsions
    float TargetAttraction = 1f;    //-1 to 1
    float DroneAttraction = 1f;     //-1 to 1
    #endregion

    void Start()
    {
        force = Vector3.zero;
        dragVector = new Vector3(Drag, Drag, Drag);
        rigidbody = transform.gameObject.GetComponent<Rigidbody>();
        hasTarget = target != null;
        rigidbody.drag = Drag;


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

    // Update is called once per frame
    void Update()
    {
        if (hasTarget) hasTarget = target != null;

        CalcAtrractions();
        //force.Scale(force.normalized);
        var test = force.magnitude;
        var testsqr = force.sqrMagnitude;
        var testnorm = force.normalized;
        var one = testnorm.magnitude;
        var two = testnorm.sqrMagnitude;
        
        if (force.magnitude >= maxAcceleration) force =  force / (force.magnitude - maxAcceleration);
        transform.Translate(force * Time.deltaTime);
        force *= (1 - Drag);

        DrawLine(transform.position, force);
        //CalcActualForce();
    }
    public void CalcAtrractions()
    {
        if(hasTarget) CalcTargetAttraction();
    }
    public void CalcTargetAttraction()
    {
        var targetVector = (target.transform.position - transform.position).normalized;
        force += targetVector * TargetAttraction;

    }
    public void CalcActualForce()
    {
        /*Force = Mass * Acceleration.

        Acceleration = DragForce / ObjectMass

        Velocity = InitialVelocity + 1 / 2(Acceleration * time ^ 2)

        FinalVelocity = InitialVelocity + 1 / 2((DragForce / ObjectMass) * DT ^ 2)*/
        float M = rigidbody.mass;
        Vector3 A = force * Time.deltaTime;
        Vector3 F = M * A;
        var temp1 = (Drag / rigidbody.mass);
        var temp2 = (Time.deltaTime * Time.deltaTime);

        float FinalVelocity = force.magnitude + 1 / 2 * (temp1 * temp2);
        var test = 1;
    }
    public void OnDrawGizmos()
    {
        DrawLine(transform.position, force);
    }
    public void DrawLine(Vector3 first, Vector3 second)
    {
        lineRenderer.SetPosition(0, first); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, second); //x,y and z position of the starting point of the line
    }
    public void AddForce(Vector3 newForce)
    {
        force += newForce;
    }
}
