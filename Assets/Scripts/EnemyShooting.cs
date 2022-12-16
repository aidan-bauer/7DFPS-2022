using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    [SerializeField] ParticleSystem nomralShot;
    [SerializeField] ParticleSystem damageShot;
    [SerializeField] GunData gunData;

    //[SerializeField] float damageTime = 7f;
    [SerializeField] float damageTimeVariation = 0.25f;
    [SerializeField] float maxStartDelay = 2f;


    [SerializeField] private int currentAmmo;
    public bool outOfAmmo { get; private set; }

    public event Action<int> onWeaponReload;
    public event Action<int> onWeaponFired;

    Health enemyHealth;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).LookAt(player.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.IsPaused)
        {

        }
    }

    public void Reload()
    {
        currentAmmo = gunData.ammoBeforeReload;
        outOfAmmo = false;
        enemyHealth.SetCoverStatus(false);
        StartCoroutine(StartDelay());

        if (onWeaponReload != null)
        {
            onWeaponReload.Invoke(currentAmmo);
        }
    }

    public void Shoot()
    {
        if (currentAmmo <= 0)
        {
            outOfAmmo = true;
            enemyHealth.SetCoverStatus(true);
            Reload();
            return;
        }

        if (outOfAmmo == true)
        {
            return;
        }

        currentAmmo--;

        //only damage the player on the last shot in the clip
        if (currentAmmo == 0)
        {
            IDamagable damagable = player.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Debug.Log("damanging player");
                damagable.TakeDamage(gunData.damage);
            }
            damageShot.Play();
        } else
        {
            nomralShot.Play();
        }

        StartCoroutine(ShotTiming());

        if (onWeaponFired != null)
        {
            onWeaponFired.Invoke(currentAmmo);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        //StartCoroutine(StartDelay());
        Reload();
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, maxStartDelay));
        StartCoroutine(ShotTiming());
    }

    IEnumerator ShotTiming()
    {
        //Debug.Log("beginning shot timing");
        float interval = gunData.fireRate + UnityEngine.Random.Range(-damageTimeVariation, damageTimeVariation);

        if (currentAmmo == 1)
        {
            //play warning sign for damaging shot
        }

        yield return new WaitForSeconds(interval);
        Shoot();
    }
}
