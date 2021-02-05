using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    [ShowOnly] public SphereCollider collider;
    [ShowOnly] public List<GameObject> ObjectsInRange;
    [ShowOnly] public List<string> validTags; 
    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<SphereCollider>();
        ObjectsInRange = new List<GameObject>();
        validTags = new List<string>();
    }
    private void Update()
    {
        for (int i = 0; i < ObjectsInRange.Count; i++)
        {
            if (ObjectsInRange[i] == null) ObjectsInRange.RemoveAt(i);
        }
    }
    void OnTriggerEnter(Collider other) //TODO: cantargetfriendlies
    {
        if (IsValid(other) && !ObjectsInRange.Contains(other.gameObject))
        {
            ObjectsInRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (IsValid(other) && ObjectsInRange.Contains(other.gameObject))
        {
            ObjectsInRange.Remove(other.gameObject);
        }
    }
    private bool IsValid(Collider other) => validTags.Where(tag => !tag.Equals(other.tag)).Count() <= 0;
}
