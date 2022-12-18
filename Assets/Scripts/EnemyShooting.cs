using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    [Tooltip("Quick note: Orientation here is defined as the direction the player is pointing in")]
    public enum PlayerOrientation { Up, Down, Left, Right }
    public PlayerOrientation orientation = PlayerOrientation.Up;


    [SerializeField] ParticleSystem nomralShot;
    [SerializeField] ParticleSystem damageShot;
    [SerializeField] GunData gunData;

    [SerializeField] bool onCeiling = false;
    [SerializeField] float damageTimeVariation = 0.25f;
    [SerializeField] float maxStartDelay = 2f;


    [SerializeField] private int currentAmmo;
    public bool outOfAmmo { get; private set; }

    public event Action<int> onWeaponReload;
    public event Action<int> onWeaponFired;

    Health enemyHealth;
    Animator anim;
    GameObject player;

    [HideInInspector] public Coroutine shotTimer, startDelay;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        enemyHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        enemyHealth.onDeath += OnDeath;
    }

    private void OnDisable()
    {
        enemyHealth.onDeath -= OnDeath;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 lookVector = Vector3.zero;

        switch (orientation)
        {
            case PlayerOrientation.Up: 
                lookVector = Vector3.up;
                break;
            case PlayerOrientation.Down:
                lookVector = Vector3.down;
                break;
            case PlayerOrientation.Right:
                lookVector = Vector3.right;
                break;
            case PlayerOrientation.Left:
                lookVector = Vector3.left;
                break;
        }

        transform.GetChild(0).LookAt(player.transform.position, lookVector);
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
        startDelay = StartCoroutine(StartDelay());

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
                //Debug.Log("damanging player");
                damagable.TakeDamage(gunData.damage);
            }
            anim.Rebind();
            damageShot.Play();
        } else
        {
            nomralShot.Play();
        }

        shotTimer = StartCoroutine(ShotTiming());

        if (onWeaponFired != null)
        {
            onWeaponFired.Invoke(currentAmmo);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        Reload();
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, maxStartDelay));
        shotTimer = StartCoroutine(ShotTiming());
    }

    IEnumerator ShotTiming()
    {
        //Debug.Log("beginning shot timing");
        float interval = gunData.fireRate + UnityEngine.Random.Range(-damageTimeVariation, damageTimeVariation);

        if (currentAmmo == 1)
        {
            //play warning sign for damaging shot
            anim.Play("warning");
        }

        yield return new WaitForSeconds(interval);
        Shoot();
    }

    public void OnDeath(Health health)
    {
        StopCoroutine(shotTimer);
        StopCoroutine(startDelay);
        //reset all animations then play death animation
        anim.Rebind();
        //anim.SetTrigger("onDeath");
        anim.Play("death");
    }
}
