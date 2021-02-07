using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_FDE : MonoBehaviour
{
    public GameObject target;
    [ShowOnly] public bool hasTarget;
    [ShowOnly] public Vector3 force;
    public float Drag = 0.10f;
    public float maxAcceleration = 50.0f;
    public float speed = 12f;
    private Vector3 dragVector;
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    public TriggerCollider TriggerCollider;
    [ShowOnly] public float wander_angle = 0; //degrees
    #region Attractions and repulsions
    [ShowOnly] public Drone_dna Dna;
    [ShowOnly] public RaycastHit Bottom;
    [ShowOnly] public RaycastHit Top;
    [ShowOnly] public RaycastHit Forward;
    [ShowOnly] public RaycastHit Right;
    [ShowOnly] public RaycastHit Backward;
    [ShowOnly] public RaycastHit Left;
    #endregion

    void Start()
    {
        force = Vector3.zero;
        dragVector = new Vector3(Drag, Drag, Drag);
        rigidbody = transform.gameObject.GetComponent<Rigidbody>();
        hasTarget = target != null;
        rigidbody.drag = Drag;
        TriggerCollider = gameObject.GetComponent<TriggerCollider>();
        if (TriggerCollider == null) Debug.LogError("TriggerCollider not properly set up.");
        TriggerCollider.validTags.Add("Soldier");

        Dna = gameObject.AddComponent<Drone_dna>();
        Dna.GenerateHardCodedDNA();

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

    void Update()
    {
        hasTarget = target != null;
        force = Vector3.zero;
        CalcAtrractions();
        CalcRayCasts();
        CalcRayAttractions();
        force = force.normalized * speed;
        transform.Translate(force * Time.deltaTime);

        DrawLine(transform.position, transform.position + force);
    }
    #region FDE forces
    public void CalcAtrractions()
    {
        if (hasTarget) CalcTargetAttraction();
        else Wander();
        if(TriggerCollider.ObjectsInRange.Count > 0)
        {
            CalcCohesion();
            CalcSeperation();
            CalcAlignment();
        }
    }
    public void CalcTargetAttraction()
    {
        Vector3 targetVector = (target.transform.position - transform.position).normalized;
        force += targetVector * Dna.TargetAttraction;
    }
    public void CalcCohesion()
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < TriggerCollider.ObjectsInRange.Count; i++)
        {
            center += TriggerCollider.ObjectsInRange[i].transform.position;
        }
        center /= TriggerCollider.ObjectsInRange.Count;

        force += (center - transform.position).normalized * Dna.Cohesion;
    }
    public void CalcSeperation()
    {
        Vector3 seperation = Vector3.zero;

        for (int i = 0; i < TriggerCollider.ObjectsInRange.Count; i++)
        {
            seperation += (transform.position - TriggerCollider.ObjectsInRange[i].transform.position).normalized;
        }

        force += (seperation.normalized * Dna.Seperation);
    }
    public void CalcAlignment()
    {
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < TriggerCollider.ObjectsInRange.Count; i++)
        {
            var test = (TriggerCollider.ObjectsInRange[i].transform.position + TriggerCollider.ObjectsInRange[i].transform.forward).normalized; //TODO:
            alignment += TriggerCollider.ObjectsInRange[i].transform.position + TriggerCollider.ObjectsInRange[i].transform.forward;
        }

        alignment /= TriggerCollider.ObjectsInRange.Count;

        force += alignment.normalized * Dna.Alignment;
    }
    public void Wander()
    {
        float wander_radius = 5f;
        Vector3 center = transform.forward;
        var size = rigidbody.velocity.normalized;

        if(size.magnitude > 0f)
        {
            center = size / force.magnitude * wander_radius;
        }

        Vector3 displacement = new Vector3(Mathf.Cos(wander_angle) * -wander_radius, 0, Mathf.Sin(wander_angle) * -wander_radius);
        float distance = 0.001f;
        wander_angle += Mathf.Rad2Deg * Random.Range(-distance * Mathf.PI, distance * Mathf.PI);
        float wander = 0.02f;
        force += (center + displacement) * wander;
    }
    #endregion
    public void CalcRayAttractions()
    {
        if (Top.point.magnitude > 0) force += new Vector3(0, 1 / (transform.position.y - Top.point.y) * -Dna.TopAttr, 0);
        if (Bottom.point.magnitude > 0) force += new Vector3(0, 1 / (transform.position.y - Bottom.point.y) * -Dna.BottomAttr, 0);
        if (Left.point.magnitude > 0) force += new Vector3(1 / (transform.position.x - Left.point.x) * -Dna.LeftAttr, 0, 0);
        if (Right.point.magnitude > 0) force += new Vector3(1 / (transform.position.x - Right.point.x) * -Dna.RightAttr, 0, 0);
        if (Forward.point.magnitude > 0) force += new Vector3(0, 0, 1 / (transform.position.z - Forward.point.z) * -Dna.ForwardAttr);
        if (Backward.point.magnitude > 0) force += new Vector3(0, 0, 1 / (transform.position.z - Backward.point.z) * -Dna.BackAttr);
    }
    public void CalcRayCasts()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        layerMask |= 1 << 9;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        float horizontalConstant = 0.01f;
        float verticalConstant = 0.01f;
        float rayLength = 3;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out Top, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * Top.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * rayLength, Color.green);

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out Bottom, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up * -1) * Bottom.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up * -1) * rayLength, Color.yellow);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right * -1), out Left, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right * -1) * Left.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right * -1) * rayLength, Color.yellow);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Forward, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Forward.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayLength, Color.blue);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out Right, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * Right.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * rayLength, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward * -1), out Backward, rayLength, layerMask))
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * -1) * Backward.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * -1) * rayLength, Color.yellow);

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
