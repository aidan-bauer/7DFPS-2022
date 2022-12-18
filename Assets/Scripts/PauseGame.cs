using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private UnityEvent onPause;
    [Space]
    [Header("Un Pause")]
    [SerializeField] private UnityEvent onUnPause;


    private bool isPaused = false;
    public void OnPause(InputValue value)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            onPause.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            onUnPause.Invoke();
        }
    }
}
