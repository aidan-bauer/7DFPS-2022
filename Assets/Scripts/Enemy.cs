using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]private Health health;

    private void OnEnable()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
        }

        health.onTakeDamage += ReactToDamage;
    }

    private void OnDisable()
    {
        health.onTakeDamage -= ReactToDamage;
    }


    public void ReactToDamage(int damage)
    {
        Debug.Log(gameObject.name + " has been shot for " + damage + " damage");
    }


}
