using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHitscan : MonoBehaviour, IGun
{
    [Header("Object References")]
    [SerializeField] Camera camera;

    [SerializeField] GunData gunData;


    [SerializeField] private int currentAmmo;
    public bool outOfAmmo { get; private set; }

    
    public event Action<int> onWeaponReload;   
    public event Action onWeaponMisfire;
    public event Action<int> onWeaponFired;
    public event Action<GunData> onWeaponEquip;

    public void Equip()
    {
        currentAmmo = gunData.ammoBeforeReload;
        outOfAmmo = false;
        onWeaponEquip.Invoke(gunData);
    }

    public void Misfire()
    {
        onWeaponMisfire.Invoke();
    }

    public void Reload()
    {
        currentAmmo = gunData.ammoBeforeReload;
        outOfAmmo = false;
        //onWeaponReload.Invoke(currentAmmo);
    }

    public void Shoot(Vector3 fireLocation)
    {
        if(currentAmmo <= 0)
        {
            outOfAmmo = true;
            Debug.Log("Out of ammo. Please reload");
        }

        if(outOfAmmo == true)
        {
            return;
        }

        currentAmmo--;

        Ray ray = camera.ScreenPointToRay(fireLocation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            IDamagable damagable = hit.collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(gunData.damage);
            }
        }

        //onWeaponFired.Invoke(currentAmmo);
    }

    public GunData GunData()
    {
        return gunData;
    }

}
