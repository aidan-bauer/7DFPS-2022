using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    IGun currentGun;
    private GunData gunData;

    public GunTester gunTester; //Temporary, this would be a reference to whatever holds the players current gun

    [Header("UI Object References")]
    [SerializeField] Image gunImage;
    [SerializeField] Image ammoImage;
    [SerializeField] TextMeshProUGUI ammoRemaining;
    [SerializeField] TextMeshProUGUI maxAmmo;
    [SerializeField] TextMeshProUGUI divider;


    private void OnEnable()
    {
        if(gunTester == null)
        {
            gunTester = FindObjectOfType<GunTester>();
        }
        gunTester.onGunEquip += UpdateCurrentWeapon;
    }

    private void OnDisable()
    {
        gunTester.onGunEquip -= UpdateCurrentWeapon;
    }

    private void UpdateCurrentWeapon(IGun newGun)
    {
        //Unsubscribe from previous weapon Actions
        if(currentGun != null)
        {
            currentGun.onWeaponEquip -= UpdateGunData;
            currentGun.onWeaponFired -= UpdateAmmo;
            currentGun.onWeaponReload -= UpdateAmmo;
        }

        //Subscribe to new weapon Actions
        currentGun = newGun;
        currentGun.onWeaponEquip += UpdateGunData;
        currentGun.onWeaponFired += UpdateAmmo;
        currentGun.onWeaponReload += UpdateAmmo;
        
    }

    private void UpdateGunData(GunData newData)
    {
        gunData = newData;
        gunImage.sprite = gunData.gunImage;
        ammoImage.sprite = gunData.ammoImage;
        ammoRemaining.text = gunData.ammoBeforeReload.ToString();
        maxAmmo.text = gunData.ammoBeforeReload.ToString();

    }

    private void UpdateAmmo(int currentAmmo)
    {
        ammoRemaining.text = currentAmmo.ToString();
    }
}
