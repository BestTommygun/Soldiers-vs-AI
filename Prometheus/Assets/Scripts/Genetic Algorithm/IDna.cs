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
    IDna GenerateChild(IDna parent1, IDna parent2, float mutationChance = 5f);

    void GenerateRandomDNA(DnaGenMode dnaGenMode = DnaGenMode.All);
}
