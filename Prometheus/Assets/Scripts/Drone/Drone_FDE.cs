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
    public float speed = 30;
    private Vector3 dragVector;
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    public List<GameObject> perceivedDrones;
    [ShowOnly] public float wander_angle = 0; //degrees
    #region Attractions and repulsions
    [Header("FDE attractions")]
    [ShowOnly] public float TargetAttraction = 1f;    //-1 to 1
    [ShowOnly] public float DroneAttraction = 1f;    //-1 to 1
    [ShowOnly] public float Cohesion = 0f;     //-1 to 1
    [ShowOnly] public float Seperation = 1f;     //-1 to 1
    [ShowOnly] public float Alignment = 1f;     //-1 to 1

    [Header("Movement attractions")]
    [ShowOnly] public float TopAttr = 1f;    //-1 to 1
    [ShowOnly] public float BottomAttr = 1f;    //-1 to 1
    [ShowOnly] public float LeftAttr = 1f;     //-1 to 1
    [ShowOnly] public float ForwardAttr = 1f;     //-1 to 1
    [ShowOnly] public float RightAttr = 1f;
    [ShowOnly] public float BackAttr = 1f;

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
        perceivedDrones = new List<GameObject>();

        TargetAttraction    = 1f;
        DroneAttraction     = 1f;
        Cohesion            = 0.12f;
        Seperation          = 0.23f;
        Alignment           = 0.1f;

        TopAttr     = -0.2f;
        BottomAttr  = -0.2f;
        LeftAttr    = -0.1f;
        ForwardAttr = -0.1f;
        RightAttr   = -0.1f;
        BackAttr    = -0.1f;

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
        if (hasTarget) hasTarget = target != null;
        for (int i = 0; i < perceivedDrones.Count; i++)
        {
            if (perceivedDrones[i] == null) perceivedDrones.RemoveAt(i);
        }

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
        if(perceivedDrones.Count > 0)
        {
            CalcCohesion();
            CalcSeperation();
            CalcAlignment();
        }
    }
    public void CalcTargetAttraction()
    {
        Vector3 targetVector = (target.transform.position - transform.position).normalized;
        force += targetVector * TargetAttraction;
    }
    public void CalcCohesion()
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < perceivedDrones.Count; i++)
        {
            center += perceivedDrones[i].transform.position;
        }
        center /= perceivedDrones.Count;

        force += (center - transform.position).normalized * Cohesion;
    }
    public void CalcSeperation()
    {
        Vector3 seperation = Vector3.zero;

        for (int i = 0; i < perceivedDrones.Count; i++)
        {
            seperation += (transform.position - perceivedDrones[i].transform.position).normalized;
        }

        force += (seperation.normalized * Seperation);
    }
    public void CalcAlignment()
    {
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < perceivedDrones.Count; i++)
        {
            var test = (perceivedDrones[i].transform.position + perceivedDrones[i].transform.forward).normalized;
            alignment += perceivedDrones[i].transform.position + perceivedDrones[i].transform.forward;
        }

        alignment /= perceivedDrones.Count;

        force += alignment.normalized * Alignment;
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

        force += (center + displacement);
    }
    #endregion
    public void CalcRayAttractions()
    {
        //up, down, left, forward, right, backward
        if (Top.point.magnitude > 0) force += Vector3.MoveTowards(force, Top.point, -1 * TopAttr * Time.deltaTime);
        if (Bottom.point.magnitude > 0) force += Vector3.MoveTowards(force, transform.position - Bottom.point, -1 * BottomAttr);
        if (Left.point.magnitude > 0) force += Vector3.MoveTowards(force, transform.position - Left.point, -1 * LeftAttr);
        if (Forward.point.magnitude > 0) force += Vector3.MoveTowards(force, transform.position - Forward.point, -1 * ForwardAttr);
        if (Right.point.magnitude > 0) force += Vector3.MoveTowards(force, transform.position - Right.point, -1 * RightAttr);
        if (Backward.point.magnitude > 0) force += Vector3.MoveTowards(force, transform.position - Backward.point, -1 * BackAttr);
    }
    public void CalcRayCasts()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        float horizontalConstant = 0.75f;
        float verticalConstant = 0.01f;
        float rayLength = 3;
        if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.up) * verticalConstant, transform.TransformDirection(Vector3.up), out Top, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.up) * verticalConstant, transform.TransformDirection(Vector3.up) * Top.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.up) * verticalConstant, transform.TransformDirection(Vector3.up) * rayLength, Color.yellow);
        if(Physics.Raycast(transform.position + transform.TransformDirection(Vector3.up * -1) * verticalConstant, transform.TransformDirection(Vector3.up * -1), out Bottom, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.up * -1) * verticalConstant, transform.TransformDirection(Vector3.up * -1) * Bottom.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.up * -1) * verticalConstant, transform.TransformDirection(Vector3.up * -1) * rayLength, Color.yellow);
        if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.right * -1) * horizontalConstant, transform.TransformDirection(Vector3.right * -1), out Left, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.right * -1) * horizontalConstant, transform.TransformDirection(Vector3.right * -1) * Left.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.right * -1) * horizontalConstant, transform.TransformDirection(Vector3.right * -1) * rayLength, Color.yellow);
        if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.forward) * horizontalConstant, transform.TransformDirection(Vector3.forward), out Forward, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.forward) * horizontalConstant, transform.TransformDirection(Vector3.forward) * Forward.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.forward) * horizontalConstant, transform.TransformDirection(Vector3.forward) * rayLength, Color.yellow);
        if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.right) * horizontalConstant, transform.TransformDirection(Vector3.right), out Right, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.right) * horizontalConstant, transform.TransformDirection(Vector3.right) * Right.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.right) * horizontalConstant, transform.TransformDirection(Vector3.right) * rayLength, Color.yellow);
        if (Physics.Raycast(transform.position + transform.TransformDirection(Vector3.forward * -1) * horizontalConstant, transform.TransformDirection(Vector3.forward * -1), out Backward, rayLength, layerMask))
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.forward * -1) * horizontalConstant, transform.TransformDirection(Vector3.forward * -1) * Backward.distance, Color.cyan);
        else
            Debug.DrawRay(transform.position + transform.TransformDirection(Vector3.forward * -1) * horizontalConstant, transform.TransformDirection(Vector3.forward * -1) * rayLength, Color.yellow);
    }
    public void DrawLine(Vector3 first, Vector3 second)
    {
        lineRenderer.SetPosition(0, first); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, second); //x,y and z position of the starting point of the line
    }
    void OnTriggerEnter(Collider other) //TODO: cantargetfriendlies
    {
        if (IsDrone(other) && !perceivedDrones.Contains(other.gameObject))
        {
            perceivedDrones.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (IsDrone(other) && perceivedDrones.Contains(other.gameObject))
        {
            perceivedDrones.Remove(other.gameObject);
        }
    }
    public void AddForce(Vector3 newForce)
    {
        force += newForce;
    }
    private bool IsDrone(Collider other) => other.tag == tag;
}
