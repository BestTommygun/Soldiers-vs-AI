using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Soldiers.Weapons
{
    public class Rifle : BaseSoldierWeapon
    {
        private RaycastHit raycastHit;
        public GameObject bulletPrefab;
        private float BulletSpeed = 100f;
        private void Start()
        {
            CurAmmo = MaxAmmo;
        }
        private void Update()
        {
            checkTimers();
            Debug.DrawRay(barrelEnd.transform.position, barrelEnd.transform.forward, Color.green);
        }
        public override bool CanHitTarget(GameObject target)
        {
            return Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out raycastHit, Range);
        }
        public override void checkTimers()
        {
            if (CurCooldown > 0) CurCooldown -= Time.deltaTime;
            if (CurReloadTime > 0) CurReloadTime -= Time.deltaTime;
        }

        public override void Reload()
        {
            CurReloadTime = MaxReloadTime;

            CurAmmo = MaxAmmo;
        }

        public override RaycastHit ShootWeapon(float inaccuracy)
        {
            if(CurAmmo > 0 && CurCooldown <= 0 && CurReloadTime <= 0)
            {
                if (bulletPrefab)
                {
                    GameObject bullet = GameObject.Instantiate(bulletPrefab);
                    var projScript = bullet.GetComponent<ProjectileScript>();
                    projScript.Damage = 0;
                    projScript.Speed = BulletSpeed;
                    projScript.LifeTime = 5f;
                    projScript.ignoredLayers.Add(9); //9 = Soldier

                    Vector3 bulletposition = barrelEnd.transform.position + barrelEnd.transform.forward * 0.5f;
                    bullet.transform.SetPositionAndRotation(bulletposition, barrelEnd.transform.rotation);

                    CurAmmo--;
                    CurCooldown = MaxCooldown;
                }
            }
            return raycastHit;
        }
    }
}
