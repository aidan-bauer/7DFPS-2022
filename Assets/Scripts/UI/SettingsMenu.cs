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



    //public static event Action<float> onP1SensitivityChanged;
    //public static event Action<float> onP2SensitivityChanged;

    private void OnEnable()
    {
        Player1Sensitivity.onValueChanged.AddListener(UpdateP1Sensitivity);
        Player2Sensitivity.onValueChanged.AddListener(UpdateP2Sensitivity);
    }

    private void OnDisable()
    {
        Player1Sensitivity.onValueChanged.RemoveListener(UpdateP1Sensitivity);
        Player2Sensitivity.onValueChanged.RemoveListener(UpdateP2Sensitivity);
    }

    private void Start()
    {
        Player1Sensitivity.maxValue = Manager.constants.maxAimSensitivity;
        Player2Sensitivity.maxValue = Manager.constants.maxAimSensitivity;
    }

    public void UpdateP1Sensitivity(float sensitivity)
    {
        /*if(onP1SensitivityChanged != null)
        {
            onP1SensitivityChanged.Invoke(sensitivity);
        }*/
        PlayerPrefs.SetFloat("player1Sens", sensitivity);
    }

    public void UpdateP2Sensitivity(float sensitivity)
    {
        /*if (onP2SensitivityChanged != null)
        {
            onP2SensitivityChanged.Invoke(sensitivity);
        }*/
        PlayerPrefs.SetFloat("player2Sens", sensitivity);
    }

    public void UpdateSensitivityUI()
    {
        Debug.Log("updated sense from prefs");
        Player1Sensitivity.value = PlayerPrefs.GetFloat("player1Sens", Manager.constants.maxAimSensitivity / 2f);
        Player2Sensitivity.value = PlayerPrefs.GetFloat("player2Sens", Manager.constants.maxAimSensitivity / 2f);
    }
}
