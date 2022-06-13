using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameEventSO endGameByTimeEvent;
    [SerializeField] private GameEventSO countdownZeroEvent;
    [SerializeField] private int startCountdownTime;
    private bool isTimer = false;
    private float countdownLength = 0.0f;
    private float timerLength = 0.0f;
    private bool isCountdown = false;

    // Update is called once per frame
    void Update()
    {
        if(isTimer)
        {
            DisplayTime();
            timerLength -= Time.deltaTime;
            if(timerLength <= 0)
            {
                timerLength = 0.0f;
                endGameByTimeEvent.Raise();
                PauseTimer();
            }
        }
        if(isCountdown)
        {
            DisplayCountdown();
            countdownLength -= Time.deltaTime;
            if(countdownLength <= 0)
            {
                countdownLength = 0.0f;
                countdownZeroEvent.Raise();
                DisableCountdownText();
                PauseCountdown();
            }
        }
    }

    private void EnableCountdownText()
    {
        countdownText.gameObject.SetActive(true);
    }

    private void DisableCountdownText()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        isTimer = true;
    }

    public void PauseTimer()
    {
        isTimer = false;
    }
    
    private void DisplayTime()
    {
        timerText.text = TimeSpan.FromSeconds(timerLength).ToString("mm\\:ss");
    }
    private void DisplayCountdown()
    {
        countdownText.text = countdownLength.ToString("0");
    }

    public void SetTimerLength(float length)
    {
        timerLength = length;
    }

    public void ResetCountdownTimer()
    {
        countdownLength = startCountdownTime;
    }

    public void StartCountdown()
    {
        if(countdownLength > 0)
        {
            EnableCountdownText();
            isCountdown = true;
        }
    }
    public void PauseCountdown()
    {
        isCountdown = false;
    }
}
