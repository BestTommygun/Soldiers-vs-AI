using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTargetting : MonoBehaviour
{
    private Drone_FDE drone_fde;
    public SphereCollider perceptionCollider;
    [ShowOnly]
    public float PerceptionRange = 12;
    public bool canTargetFriendlies = false;
    [ShowOnly]
    public GameObject curTarget;
    [ShowOnly]
    public bool HasTarget;
    [ShowOnly]
    public List<GameObject> potentialTargets;
    void Start()
    {
        drone_fde = transform.GetComponent<Drone_FDE>();

        perceptionCollider = transform.GetComponent<SphereCollider>();
        PerceptionRange = perceptionCollider.radius;
        if (perceptionCollider == null) Debug.LogError("Drone was not given a perception range.");
    }

    void Update()
    {
        if (potentialTargets.Count > 0)
        {
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                if (potentialTargets[i] == null) potentialTargets.RemoveAt(i);
            }
            HasTarget = curTarget != null;
            ScanForTargets();
        }
    }

    private void ScanForTargets()
    {
        for (int i = 0; i < potentialTargets.Count; i++)
        {
            Vector3 potentialTargetPos = potentialTargets[i].transform.position;
            Vector3 curTargetPos;
            if (HasTarget) curTargetPos = curTarget.transform.position;
            else
            {
                SetNewTarget(potentialTargets[i]);
                return;
            }

            float distanceToCurTarget = (transform.position - curTargetPos).magnitude;
            float distanceToPotTarget = (transform.position - potentialTargetPos).magnitude;
            if(distanceToPotTarget < distanceToCurTarget)
            {
                SetNewTarget(potentialTargets[i]);
            }
        }
    }
    public void ConsiderTarget(GameObject target)
    {
        if (!HasTarget) SetNewTarget(target);
    }
    private void SetNewTarget(GameObject target)
    {
        curTarget = target;
        HasTarget = curTarget != null;
        drone_fde.target = curTarget;
    }
    void OnTriggerEnter(Collider other) //TODO: cantargetfriendlies
    {
        if (IsValidTarget(other) && !potentialTargets.Contains(other.gameObject))
        {
            potentialTargets.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (IsValidTarget(other) && potentialTargets.Contains(other.gameObject))
        {
            if (potentialTargets.Contains(curTarget)) SetNewTarget(null);
            potentialTargets.Remove(other.gameObject);
        }
    }
    private bool IsValidTarget(Collider other) => other.tag != tag && other.tag == "Soldier";
}
