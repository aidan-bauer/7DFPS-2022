using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Object References")]
    [SerializeField] Slider Player1Sensitivity;
    [SerializeField] Slider Player2Sensitivity;
    [SerializeField] Toggle autoAim;

    

    public event Action<float> onSensitivityChanged;

    private void OnEnable()
    {
        onSensitivityChanged += UpdateSensitivity;   
    }

    public void UpdateSensitivity(float sensitivity)
    {
        onSensitivityChanged.Invoke(sensitivity);
    }
}
