using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetTotalTimeString()
    {
        return TimeSpan.FromSeconds(0).ToString("mm\\:ss");
    }
    private void DisplayTime()
    {
        timerText.text = TimeSpan.FromSeconds(0).ToString("mm\\:ss");
    }
}
