using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Gun : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GunData gunData;
        [SerializeField] private Transform muzzle;

        float timeSinceLastShot;

        private void Start() 
        {
            FirstPersonController.shootInput += Shoot;
            FirstPersonController.reloadInput += StartReload;
        }

        public void StartReload() 
        {
        if (!gunData.reloading)
            StartCoroutine(Reload());
        }

        private IEnumerator Reload() 
        {
            gunData.reloading = true;

            yield return new WaitForSeconds(gunData.reloadTime);

            gunData.currentAmmo = gunData.magSize;

            gunData.reloading = false;
        }

        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

        public void Shoot() 
        {
            if (gunData.currentAmmo > 0) 
            {
                if (CanShoot()) 
                {
                    if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                    {
                        Debug.Log("Current Target: " + hitInfo.transform.name);
                        // takeDamage()
                    }

                    gunData.currentAmmo--;
                    timeSinceLastShot = 0;
                    OnGunShot();
                }
            }
        }

        private void Update()
        {
            timeSinceLastShot += Time.deltaTime;

            Debug.DrawLine(muzzle.position, muzzle.forward * gunData.maxDistance, Color.red);
        }

        private void OnGunShot() 
        {
            //Debug.Log("OnGunShot() called!");
        }
    }
}
