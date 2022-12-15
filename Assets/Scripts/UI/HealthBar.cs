using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Health health;
    [SerializeField] private GameObject player;
    [Header("UI Object references")]
    [SerializeField] Image healthBarImage;
    [SerializeField] TextMeshProUGUI healthText;

    private void Awake()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerShooting>().gameObject;
        }
        health = player.GetComponent<Health>();

        UpdateHealthBar(0);
    }

    private void OnEnable()
    {
        health.onTakeDamage += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.onTakeDamage -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int damage)
    {
        healthBarImage.fillAmount = health.currentHealth;
        healthText.text = health.currentHealth.ToString();
    }
}
