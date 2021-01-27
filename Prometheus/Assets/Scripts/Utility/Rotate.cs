using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public GameObject Centerpoint;
    public float Angle = 3;
    public bool X, Y, Z;
    private bool HasCenter;
    void Start()
    {
        HasCenter = Centerpoint != null;
    }
    void Update()
    {
        if(HasCenter)
        {
            transform.RotateAround(Centerpoint.transform.position, transform.position, Angle);
        }
        else
        {
            transform.Rotate(0, Angle, 0);
            
        }
    }
}
