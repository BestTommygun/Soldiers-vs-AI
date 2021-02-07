using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomExtensions;

public class Drone_dna : MonoBehaviour, IDna
{
    #region Attractions and repulsions
    [Header("FDE attractions")]
    [ShowOnly] public float TargetAttraction = 1f;  //-1 to 1
    [ShowOnly] public float DroneAttraction = 1f;   //-1 to 1
    [ShowOnly] public float Cohesion = 0f;          //-1 to 1
    [ShowOnly] public float Seperation = 1f;        //-1 to 1
    [ShowOnly] public float Alignment = 1f;         //-1 to 1

    [Header("Movement attractions")]
    [ShowOnly] public float TopAttr = 1f;       //-1 to 1
    [ShowOnly] public float BottomAttr = 1f;    //-1 to 1
    [ShowOnly] public float LeftAttr = 1f;      //-1 to 1
    [ShowOnly] public float ForwardAttr = 1f;   //-1 to 1
    [ShowOnly] public float RightAttr = 1f;     //-1 to 1
    [ShowOnly] public float BackAttr = 1f;      //-1 to 1
    public IDna GenerateChild(IDna parent1, IDna parent2, float mutationChance = 5f)
    {
        if(parent1.GetType().Equals(typeof(Drone_dna)) && parent2.GetType().Equals(typeof(Drone_dna)))
        {
            Drone_dna NewDNA = new Drone_dna();
            Drone_dna parent1DNA = (Drone_dna)parent1;
            Drone_dna parent2DNA = (Drone_dna)parent2;
            Random random = new Random();
            bool parentChoice = random.Bool();
            
            //FDE attractions
            if (parentChoice) NewDNA.TargetAttraction = parent1DNA.TargetAttraction;
            else NewDNA.TargetAttraction = parent2DNA.TargetAttraction;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.DroneAttraction = parent1DNA.DroneAttraction;
            else NewDNA.DroneAttraction = parent2DNA.DroneAttraction;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.Cohesion = parent1DNA.Cohesion;
            else NewDNA.Cohesion = parent2DNA.Cohesion;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.Seperation = parent1DNA.Seperation;
            else NewDNA.Seperation = parent2DNA.Seperation;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.Alignment = parent1DNA.Alignment;
            else NewDNA.Alignment = parent2DNA.Alignment;
            parentChoice = random.Bool();

            //Movement attractions
            if (parentChoice) NewDNA.TopAttr = parent1DNA.TopAttr;
            else NewDNA.TopAttr = parent2DNA.TopAttr;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.BottomAttr = parent1DNA.BottomAttr;
            else NewDNA.BottomAttr = parent2DNA.BottomAttr;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.LeftAttr = parent1DNA.LeftAttr;
            else NewDNA.LeftAttr = parent2DNA.LeftAttr;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.ForwardAttr = parent1DNA.ForwardAttr;
            else NewDNA.ForwardAttr = parent2DNA.ForwardAttr;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.RightAttr = parent1DNA.RightAttr;
            else NewDNA.RightAttr = parent2DNA.RightAttr;
            parentChoice = random.Bool();

            if (parentChoice) NewDNA.BackAttr = parent1DNA.BackAttr;
            else NewDNA.BackAttr = parent2DNA.BackAttr;

            if(mutationChance > 0)
            {
                if (mutationChance < 0) mutationChance = mutationChance * -1;

                //if we mutate, generate a random fde and movement variable.
                if(Random.Range(0, 100) <= mutationChance)
                {
                    int fdeIndex = Random.Range(0, 4);
                    int movIndex = Random.Range(0, 5);

                    switch (fdeIndex)
                    {
                        case 0: NewDNA.TargetAttraction = Random.Range(-1f, 1f);
                            break;
                        case 1: NewDNA.DroneAttraction  = Random.Range(-1f, 1f);
                            break;
                        case 2: NewDNA.Cohesion         = Random.Range(-1f, 1f);
                            break;
                        case 3: NewDNA.Seperation       = Random.Range(-1f, 1f);
                            break;
                        case 4: NewDNA.Alignment        = Random.Range(-1f, 1f);
                            break;
                    }
                    switch (movIndex)
                    {
                        case 0: NewDNA.TopAttr      = Random.Range(-1f, 1f);
                            break;
                        case 1: NewDNA.BottomAttr   = Random.Range(-1f, 1f);
                            break;
                        case 2: NewDNA.LeftAttr     = Random.Range(-1f, 1f);
                            break;
                        case 3: NewDNA.ForwardAttr  = Random.Range(-1f, 1f);
                            break;
                        case 4: NewDNA.RightAttr    = Random.Range(-1f, 1f);
                            break;
                        case 5: NewDNA.BackAttr     = Random.Range(-1f, 1f);
                            break;
                    }
                }
            }
            return NewDNA;
        }
        else  
        {
            Debug.LogError("Drone_dna was provided parents who were not both of type Drone_dna, the Drone_dna provided itself as new child.");
            return this;
        }
    }
    #endregion

    public void GenerateRandomDNA(DnaGenMode dnaGenMode = DnaGenMode.All)
    {
        if(dnaGenMode == DnaGenMode.All || dnaGenMode == DnaGenMode.FDE)
        {
            TargetAttraction    = Random.Range(-1f, 1f);
            DroneAttraction     = Random.Range(-1f, 1f);
            Cohesion            = Random.Range(-1f, 1f);
            Seperation          = Random.Range(-1f, 1f);
            Alignment           = Random.Range(-1f, 1f);
        }
        if(dnaGenMode == DnaGenMode.All || dnaGenMode == DnaGenMode.Movement)
        {
            TopAttr     = Random.Range(-1f, 1f);
            BottomAttr  = Random.Range(-1f, 1f);
            LeftAttr    = Random.Range(-1f, 1f);
            ForwardAttr = Random.Range(-1f, 1f);
            RightAttr   = Random.Range(-1f, 1f);
            BackAttr    = Random.Range(-1f, 1f);
        }
    }

    public void GenerateHardCodedDNA() //TODO: debug function, should be removed later.
    {
        TargetAttraction = 1f;
        DroneAttraction = 1f;
        Cohesion = 0.06f;
        Seperation = 0.12f;
        Alignment = 0.01f;

        TopAttr = -0.1f;
        BottomAttr = -0.1f;
        LeftAttr = -0.2f;
        ForwardAttr = -0.2f;
        RightAttr = -0.2f;
        BackAttr = -0.2f;
    }
}
