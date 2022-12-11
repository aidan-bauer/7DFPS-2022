using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    
    public void Shoot(Vector3 fireLocation);

    public void Misfire();

    public void Reload();

    public void Equip();
}
