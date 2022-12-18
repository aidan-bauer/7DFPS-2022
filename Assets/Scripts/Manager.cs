using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public GameObject gameOverScreen;

    public static GlobalConstants constants;

    //public static Action OnGameOver;

    Health playerHealth;

    private void OnEnable()
    {
        SettingsMenu.onP1SensitivityChanged += UpdateP1Sensitivity;
        SettingsMenu.onP2SensitivityChanged += UpdateP2Sensitivity;
        playerHealth.onDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        SettingsMenu.onP1SensitivityChanged -= UpdateP1Sensitivity;
        SettingsMenu.onP2SensitivityChanged -= UpdateP2Sensitivity;
        playerHealth.onDeath -= OnPlayerDeath;
    }

    private void Awake()
    {
        if (constants == null)
        {
            constants = Resources.Load<GlobalConstants>("Constants");
        }

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    void UpdateP1Sensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("player1Sens", sensitivity);
    }

    void UpdateP2Sensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("player2Sens", sensitivity);
    }

    void OnPlayerDeath(Health health)
    {
        gameOverScreen.SetActive(true);
    }
}
