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

    

    public static event Action<float> onP1SensitivityChanged;
    public static event Action<float> onP2SensitivityChanged;

    public void UpdateP1Sensitivity(float sensitivity)
    {
        if(onP1SensitivityChanged != null)
        {
            onP1SensitivityChanged.Invoke(sensitivity);
        }
    }

    public void UpdateP2Sensitivity(float sensitivity)
    {
        if (onP2SensitivityChanged != null)
        {
            onP2SensitivityChanged.Invoke(sensitivity);
        }
    }
}
