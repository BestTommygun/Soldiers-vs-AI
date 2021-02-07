using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Soldiers
{
    public interface ISoldierWeapon
    {
        RaycastHit ShootWeapon(float inaccuracy);
        void Reload();
        bool CanHitTarget(GameObject target);
        void checkTimers();
    }
}
