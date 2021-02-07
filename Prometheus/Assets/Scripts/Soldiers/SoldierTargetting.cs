using Assets.Scripts.Soldiers.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTargetting : MonoBehaviour
{
    [ShowOnly] public bool ShouldShoot;
    public float VisionRange = 25f;
    private RaycastHit visionHit;

    [ShowOnly] public List<GameObject> DronesInRange;
    [ShowOnly] public GameObject CurrentTarget;
    public BaseSoldierWeapon soldierWeapon;
    public TriggerCollider triggerCollider;
    void Start()
    {

        ShouldShoot = false;
        DronesInRange = new List<GameObject>();
        if(triggerCollider != null)
        triggerCollider.validTags.Add("Drone");
    }

    void Update()
    {
        //cleanup
        for (int i = 0; i < DronesInRange.Count; i++)
        {
            if (DronesInRange[i] == null) DronesInRange.RemoveAt(i);
        }

        //targetting
        FindValidTarget();
        ShouldShoot = CurrentTarget != null;
        if (ShouldShoot)
        {
            TurnToTarget();
            soldierWeapon.ShootWeapon(0.3f); //TODO: make this dependent on the soldier itself
        }
    }
    private void TurnToTarget()
    {
        Transform tarTrans = CurrentTarget.transform;
        tarTrans.position = new Vector3(tarTrans.position.x, transform.position.y, tarTrans.position.z);
        transform.LookAt(tarTrans);
    }
    private void FindValidTarget()
    {
        float targetDistance = float.MaxValue;
        bool canSeeTarget = false;
        if (CurrentTarget != null)
        {
            targetDistance = GetDistanceToTransform(CurrentTarget);
            canSeeTarget = Physics.Raycast(transform.position, CurrentTarget.transform.position, out visionHit, VisionRange);
        }
        for (int i = 0; i < DronesInRange.Count; i++)
        {
            if(DronesInRange[i] != null)
            {
                float newDistance = GetDistanceToTransform(DronesInRange[i]);
                if (newDistance < targetDistance) //we check if the object is closer, now check if we can actually see them
                {
                    bool canSeeNewTarget = Physics.Raycast(transform.position, DronesInRange[i].transform.position, out visionHit, VisionRange);
                    if (canSeeNewTarget)
                    {
                        CurrentTarget = DronesInRange[i];
                        canSeeTarget = canSeeNewTarget;
                    }
                    else if (!canSeeTarget) CurrentTarget = null; //if we cannot see either target, remove the currenttarget
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsDrone(other) && !DronesInRange.Contains(other.gameObject))
        {
            DronesInRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (IsDrone(other) && DronesInRange.Contains(other.gameObject))
        {
            DronesInRange.Remove(other.gameObject);
        }
    }
    private bool IsDrone(Collider other) => other.tag == "Drone";
    private float GetDistanceToTransform(GameObject other) => (transform.position - other.transform.position).magnitude;
}
