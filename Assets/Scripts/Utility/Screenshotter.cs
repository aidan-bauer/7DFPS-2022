using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Screenshotter : MonoBehaviour
{
    public string picName = "SomeLevel";
    public bool includeTime = false;

    InputHandler handler;

    private void Awake()
    {
        handler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        handler.Screenshot += TakeScreenshot;
    }

    private void OnDisable()
    {
        handler.Screenshot -= TakeScreenshot;
    }

    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + (includeTime ? System.DateTime.Now.ToString("MMddyyyy_HHmmss") + "_" : "") + picName + ".png");
        Debug.Log("Screenshot saved to: " + Application.persistentDataPath + "/" + (includeTime ? System.DateTime.Now.ToString("MMddyyyy_HHmmss") + "_" : "") + picName + ".png");
    }
}
