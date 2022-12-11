using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTester : MonoBehaviour
{
    public GameObject gameObject;    

    private IGun gun;

    private void Start()
    {
        gun = gameObject.GetComponent<IGun>();
        Debug.Log(gun);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot(Input.mousePosition);
        }
    }
}
