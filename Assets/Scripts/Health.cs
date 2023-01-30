using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] AudioClip hurtSFX;
    public bool inCover = true;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] int score = 10;
    public int currentHealth { get; private set; }

    public event Action<int> onTakeDamage;
    public event Action<Health> onDeath;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        currentHealth = maxHealth;


    }

    private void OnEnable()
    {
        onDeath += OnDeath;
    }

    private void OnDisable()
    {
        onDeath -= OnDeath;
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log(transform.name + " takes " + damage);
        if (!inCover)
        {
            if (currentHealth - damage > 0)
            {
                currentHealth -= damage;

                if (onTakeDamage != null)
                {
                    source.PlayOneShot(hurtSFX);
                    onTakeDamage.Invoke(damage);
                }
            }
            else
            {
                //invoke death code
                if (onDeath != null)
                {
                    onDeath.Invoke(this);
                }
            }
        }
    }

    public void SetCoverStatus(bool newCover)
    {
        inCover = newCover;
    }

    void OnDeath(Health health)
    {
        //Debug.Log(health.gameObject.name + " is deded");
        if (!transform.CompareTag("Player"))
        {
            ScoreManager.ScoreChange.Invoke(score);

            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
        } else
        {
            GetComponent<PlayerShooting>().canPlayerShoot = false;
        }
    }
}
