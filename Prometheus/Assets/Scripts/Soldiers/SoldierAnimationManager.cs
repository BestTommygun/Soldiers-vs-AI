using Assets.Scripts.Soldiers.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimationManager : MonoBehaviour
{
    public Animator animator;
    public HealthManager healthManager;
    public SoldierTargetting soldierTargetting;

    void Start()
    {
       
    }

    void Update()
    {
        animator.SetInteger("Ammo", (int)soldierTargetting.soldierWeapon.CurAmmo);
        animator.SetFloat("Health", healthManager.CurHealth);
        animator.SetBool("ShouldBeShooting", soldierTargetting.ShouldShoot);
        if(soldierTargetting.soldierWeapon.CurAmmo <= 0)
        {
            soldierTargetting.soldierWeapon.Reload();
        }
    }
}
