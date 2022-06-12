using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Canvas startMenuCanvas;
    [SerializeField] private Canvas endMenuCanvas;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private BeatManager beatManager;
    
    private FightTimer fightTimer;

    // Start is called before the first frame update
    void Start()
    {
        fightTimer = GetComponent<FightTimer>();
        Debug.Log("fightTimer");
        Debug.Log(fightTimer);
    }

    // Update is called once per frame
    void Update()
    {
        // if(hasStarted)
        // {

        // }
    }

    public void StartGame()
    {
        startMenuCanvas.enabled = false;
        var randomIndex = ChooseRandomMusicIndex();
        StartRadomMusic(randomIndex);
        StartBeatScroller(audioManager.GetCurrentSoundBpm());
        SetTimerLength(audioManager.GetCurrentSoundLength());
        StartTimer();
    }

    private void SetTimerLength(float length)
    {
        fightTimer.SetTimerLength(length);
    }

    private void StartRadomMusic(int randomIndex)
    {
        audioManager.Play(randomIndex);
    }

    private void StartBeatScroller(float soundBpm)
    {
    }

    private void StartTimer()
    {
        fightTimer.StartTimer();
    }

    private int ChooseRandomMusicIndex()
    {
       return UnityEngine.Random.Range(0, audioManager.GetSoundCount()); 
    }

    private void EndGame()
    {

    }

    private void ResetVariables()
    {

    }

    public void PlayAgain()
    {

    }
}
