using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Soldiers.Weapons
{
    public abstract class BaseSoldierWeapon : MonoBehaviour, ISoldierWeapon
    {
        public float MaxAmmo, CurAmmo, CurReloadTime, MaxReloadTime, CurCooldown, MaxCooldown;
        public float Inaccuracy;
        public float Range;
        public float Damage;
        public float BulletDrop; //TODO: maybe just throw this away.
        public GameObject barrelEnd;
        public abstract bool CanHitTarget(GameObject target);

        public abstract void Reload();

        public abstract RaycastHit ShootWeapon(float inaccuracy);
        public abstract void checkTimers();
    }
}
