using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameEventSO endGameByTimeEvent;
    private bool isTimer = false;
    private float timerLength = 0.0f;

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
            }
        }
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

    public void SetTimerLength(float length)
    {
        timerLength = length;
    }
}
