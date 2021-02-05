using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Drone_dna : MonoBehaviour, IDna
{
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
    public void GenerateChild(IDna parent1, IDna parent2)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    public void GenerateRandomDNA(DnaGenMode dnaGenMode = DnaGenMode.All)
    {
        if(dnaGenMode == DnaGenMode.All || dnaGenMode == DnaGenMode.FDE)
        {
            TargetAttraction    = Random.Range(-1, 1);
            DroneAttraction     = Random.Range(-1, 1);
            Cohesion            = Random.Range(-1, 1);
            Seperation          = Random.Range(-1, 1);
            Alignment           = Random.Range(-1, 1);
        }
        if(dnaGenMode == DnaGenMode.All || dnaGenMode == DnaGenMode.Movement)
        {
            TopAttr     = Random.Range(-1, 1);
            BottomAttr  = Random.Range(-1, 1);
            LeftAttr    = Random.Range(-1, 1);
            ForwardAttr = Random.Range(-1, 1);
            RightAttr   = Random.Range(-1, 1);
            BackAttr    = Random.Range(-1, 1);
        }
        

    }
}
