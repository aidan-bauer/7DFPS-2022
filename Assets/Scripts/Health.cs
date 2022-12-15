using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public bool inCover = true;
    [SerializeField] private int maxHealth = 100;
    public int currentHealth { get; private set; }

    public event Action<int> onTakeDamage;
    public event Action<Health> onDeath;

    private void Start()
    {
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
            ScoreManager.ScoreChange.Invoke(10);
        }
    }
}
