using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DnaGenMode
{
    All,
    FDE,
    Movement
}
public interface IDna
{
    void GenerateChild(IDna parent1, IDna parent2);
    void GenerateRandomDNA(DnaGenMode dnaGenMode = DnaGenMode.All);
}
