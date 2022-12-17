using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] GunData currentGun;
    private GunData gunData;

    public SimpleHitscan hitscan; //Temporary, this would be a reference to whatever holds the players current gun

    [Header("UI Object References")]
    [SerializeField] Image gunImage;
    [SerializeField] Image ammoImage;
    [SerializeField] TextMeshProUGUI ammoRemaining;
    [SerializeField] TextMeshProUGUI maxAmmo;
    [SerializeField] TextMeshProUGUI divider;


    private void OnEnable()
    {
        if(hitscan == null)
        {
            hitscan = GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleHitscan>();

        }
        hitscan.onWeaponEquip += UpdateCurrentWeapon;
    }

    private void OnDisable()
    {
        hitscan.onWeaponEquip -= UpdateCurrentWeapon;

        hitscan.onWeaponEquip -= UpdateGunData;
        hitscan.onWeaponFired -= UpdateAmmo;
        hitscan.onWeaponReload -= UpdateAmmo;
    }

    private void UpdateCurrentWeapon(GunData newGun)
    {
        //Unsubscribe from previous weapon Actions
        if(currentGun != null)
        {
            hitscan.onWeaponEquip -= UpdateGunData;
            hitscan.onWeaponFired -= UpdateAmmo;
            hitscan.onWeaponReload -= UpdateAmmo;
        }

        //Subscribe to new weapon Actions
        currentGun = newGun;
        hitscan.onWeaponEquip += UpdateGunData;
        hitscan.onWeaponFired += UpdateAmmo;
        hitscan.onWeaponReload += UpdateAmmo;

        UpdateGunData(currentGun);
    }

    private void UpdateGunData(GunData newData)
    {
        //Debug.Log("update gun data");
        gunData = newData;
        gunImage.sprite = gunData.gunImage;
        ammoImage.sprite = gunData.ammoImage;
        ammoRemaining.text = gunData.ammoBeforeReload.ToString();
        maxAmmo.text = gunData.ammoBeforeReload.ToString();

    }

    private void UpdateAmmo(int currentAmmo)
    {
        //Debug.Log("Update Ammo: " + currentAmmo.ToString());
        ammoRemaining.text = currentAmmo.ToString();
    }
}
