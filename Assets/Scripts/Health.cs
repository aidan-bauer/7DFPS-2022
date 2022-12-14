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

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            if(onTakeDamage != null)
            {
                onTakeDamage.Invoke(damage);
            }
        }
        else
        {
            //invoke death code
            if(onDeath != null)
            {
                onDeath.Invoke(this);
            }
        }
    }

    public void SetCoverStatus()
    {
        inCover = !inCover;
    }
}
