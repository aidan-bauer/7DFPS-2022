using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGun
{
    public event Action<int> onWeaponFired;
    public event Action<int> onWeaponReload;
    public event Action<GunData> onWeaponEquip;
    public event Action onWeaponMisfire;




    public void Shoot(Vector3 fireLocation);

    public void Misfire();

    public void Reload();

    public void Equip();
}
