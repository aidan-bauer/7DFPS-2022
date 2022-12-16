using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [Header("Time In Seconds")]
    public float timerDuration = 3f * 60f;

    private float timer;
    [Header("Text References")]
    [SerializeField]
    private TextMeshProUGUI firstMinute;
    [SerializeField]
    private TextMeshProUGUI secondtMinute;
    [SerializeField]
    private TextMeshProUGUI separator;
    [SerializeField]
    private TextMeshProUGUI firstSecond;
    [SerializeField]
    private TextMeshProUGUI secondSecond;

    private float flashTimer;
    private float flashDuration = 1f;
    [Header("End of Game Event to Be Called")]
    [SerializeField] GameEvent endOfGameTime;
    

    private bool timerPaused = false;

    public event Action onTimerZero;

    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerPaused == false)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay(timer);
            }
            else
            {
                if(onTimerZero != null)
                {
                    onTimerZero.Invoke();
                }
                Flash();
            }
        }
    }

    private void ResetTimer()
    {
        timer = timerDuration;
    }

    private void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time/60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondtMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();

    }

    public void StartStopTime(bool pause)
    {
        timerPaused = pause;
    }

    private void Flash()
    {
        if(timer!= 0)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
            endOfGameTime.Raise();
        }
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    private void SetTextDisplay(bool enabled)
    {

    }
}
