using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth { get; private set; }

    public event Action<int> onTakeDamage;
    public event Action onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            //onTakeDamage.Invoke(damage);
        }
        else
        {
            //invoke death code
            onDeath.Invoke();
        }
    }
}
