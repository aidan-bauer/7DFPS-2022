using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun")]
public class Gun : ScriptableObject
{
    
    public int damage;
    public float fireRate;

    public void Shoot()
    {
        Debug.Log(this.name + " pew pew pew");
    }

}    
