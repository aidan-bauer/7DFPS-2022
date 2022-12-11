using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTester : MonoBehaviour
{
    public GameObject gameObject;    

    public IGun gun { get; private set; }

    public event Action<IGun> onGunEquip;

    private void Start()
    {
        gun = gameObject.GetComponent<IGun>();
        gun.Equip();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gun.Reload();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            onGunEquip.Invoke(gun);
            gun.Equip();
            
        }
    }


}
