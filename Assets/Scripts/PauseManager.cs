using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    //just in case we need it
    //public static PauseManager instance;
    InputHandler inputHandler;

    public GameObject pauseMenu;
    [SerializeField] bool displayPause;
    static bool isPaused = false;

    public static bool IsPaused
    {
        get
        {
            return isPaused;
        }
    }

    private void OnEnable()
    {
        inputHandler.Pause += SetPause;
    }

    private void OnDisable()
    {
        inputHandler.Pause -= SetPause;
    }

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //isPaused = true;
        //SetTimeScale(isPaused);     //lock the cursor when level loads
    }

    private void Update()
    {
        displayPause = isPaused;
    }

    void SetPause()
    {
        isPaused = !isPaused;
        SetTimeScale(isPaused);
        //SetPauseUI(isPaused);
    }

    public void SetTimeScale(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;

        if (paused)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    //toggle UI
    public void SetPauseUI(bool paused)
    {
        pauseMenu.SetActive(IsPaused);
    }

    public void UnPauseViaUI()
    {
        isPaused = false;
        SetTimeScale(isPaused);
        SetPauseUI(isPaused);
    }

    public void EndofLevel()
    {
        isPaused = true;
        SetTimeScale(isPaused);
    }
}
