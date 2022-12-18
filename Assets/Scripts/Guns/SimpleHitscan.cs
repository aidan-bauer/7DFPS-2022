using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHitscan : MonoBehaviour, IGun
{
    [Header("Object References")]
    Camera mainCamera;
    [SerializeField] GunData gunData;


    [SerializeField] private int currentAmmo;
    public bool outOfAmmo { get; private set; }
    
    public event Action<int> onWeaponReload;   
    public event Action onWeaponMisfire;
    public event Action<int> onWeaponFired;
    public event Action<GunData> onWeaponEquip;

    AudioSource source;
    ParticleSystem system;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        system = GetComponentInChildren<ParticleSystem>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        Equip();
        Reload();
    }

    public void Equip()
    {
        currentAmmo = gunData.ammoBeforeReload;
        outOfAmmo = false;
        if(onWeaponEquip != null)
        {
            onWeaponEquip.Invoke(gunData);

        }
    }

    public void Misfire()
    {
        if(onWeaponMisfire != null)
        {
            onWeaponMisfire.Invoke();

        }
    }

    public void Reload()
    {
        currentAmmo = gunData.ammoBeforeReload;
        outOfAmmo = false;

        if(onWeaponReload != null)
        {
            onWeaponReload.Invoke(currentAmmo);
        }

        if (gunData.reloadSoundEffect)
        {
            source.PlayOneShot(gunData.reloadSoundEffect);
        }
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

        Ray ray = mainCamera.ScreenPointToRay(fireLocation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            IDamagable damagable = hit.collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(gunData.damage);
            }
        }

        if (gunData.fireSoundEffect)
        {
            source.PlayOneShot(gunData.fireSoundEffect);
        }

        system.transform.LookAt(ray.origin + ray.direction);
        system.Play();

        if (onWeaponFired != null)
        {
            onWeaponFired.Invoke(currentAmmo);
        }
    }

    public GunData GunData()
    {
        return gunData;
    }

}
