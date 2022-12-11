using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHitscan : MonoBehaviour, IGun
{

    [SerializeField] Camera camera;

    [SerializeField] GunData gunData;

    public void Equip()
    {
        throw new System.NotImplementedException();
    }

    public void Misfire()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

    public void Shoot(Vector3 fireLocation)
    {
        Ray ray = camera.ScreenPointToRay(fireLocation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(gunData.damage);
            }
        }
    }

}
