using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [ShowOnly]
    public float CurHealth;
    public float MaxHealth;
    void Start()
    {
        CurHealth = MaxHealth;
    }
    public void ChangeHealth(float deltaHealth)
    {
        CurHealth += deltaHealth;
        if (CurHealth >= MaxHealth) CurHealth = MaxHealth;
        if (CurHealth <= 0) Destroy(transform.gameObject);
    }
}
