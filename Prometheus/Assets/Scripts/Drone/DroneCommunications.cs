using Assets.Scripts.Drone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCommunications : MonoBehaviour
{
    public float commsRange = 4;
    [ShowOnly]
    public List<GameObject> DronesInRange;
    private DroneTargetting droneTargetting;
    void Start()
    {
        DronesInRange = new List<GameObject>();
        droneTargetting = transform.GetComponent<DroneTargetting>();
    }

    private void Update()
    {
        for (int i = 0; i < DronesInRange.Count; i++)
        {
            if (DronesInRange[i] == null) DronesInRange.RemoveAt(i);
        }
    }

    private void SendMessage(DroneMessage message)
    {
        for (int i = 0; i < DronesInRange.Count; i++)
        {
            DronesInRange[i].SendMessage("ReceiveMessage", message);
        }
    }

    public void ReceiveMessage(DroneMessage message)
    {
        switch (message.Type) //TODO: command pattern
        {
            case MessageType.Enemy:
                if (message.Target != null) droneTargetting.ConsiderTarget(message.Target);
                //Maybe tell other drones to also target? dont forget exit condition
                break;
            default:
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsDrone(other) && !DronesInRange.Contains(other.gameObject))
        {
            DronesInRange.Add(other.gameObject);
        }
        else if(IsValidTarget(other))
        {
            SendMessage(new DroneMessage(MessageType.Enemy, other.gameObject));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (IsDrone(other) && DronesInRange.Contains(other.gameObject))
        {
            DronesInRange.Remove(other.gameObject);
        }
    }
    private bool IsValidTarget(Collider other) => other.tag != tag && other.tag == "Soldier";
    private bool IsDrone(Collider other) => other.tag == tag;
}
