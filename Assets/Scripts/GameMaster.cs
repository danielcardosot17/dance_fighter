using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Canvas startMenuCanvas;
    [SerializeField] private Canvas endMenuCanvas;
    [SerializeField] private Canvas pauseGameCanvas;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private GameEventSO disablePlayerInputEvent;
    [SerializeField] private GameEventSO enablePlayerInputEvent;
    [SerializeField] private GameEventSO disableUIInputEvent;
    [SerializeField] private GameEventSO enableUIInputEvent;
    [SerializeField] private GameEventSO pausePlayerAnimationEvent;
    [SerializeField] private GameEventSO unPausePlayerAnimationEvent;
    
    private FightTimer fightTimer;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        fightTimer = GetComponent<FightTimer>();
        startMenuCanvas.gameObject.SetActive(true);
        endMenuCanvas.gameObject.SetActive(false);
        pauseGameCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {

        }
    }

    public void StartGame()
    {
        isPlaying = true;
        startMenuCanvas.gameObject.SetActive(false);
        endMenuCanvas.gameObject.SetActive(false);
        var randomIndex = ChooseRandomMusicIndex();
        ResetVariables();
        StartRadomMusic(randomIndex);
        StartBeatScroller(audioManager.GetCurrentSoundBpm());
        SetTimerLength(audioManager.GetCurrentSoundLength());
        ResetCountdownTimer();
        StartTimer();
        StartCountdown();
        // StartPlayers();
    }

    private void StartCountdown()
    {
        fightTimer.StartCountdown();
    }

    public void StartPlayers()
    {
        disableUIInputEvent.Raise();
        enablePlayerInputEvent.Raise();
    }

    private void SetTimerLength(float length)
    {
        fightTimer.SetTimerLength(length);
    }

    private void ResetCountdownTimer()
    {
        fightTimer.ResetCountdownTimer();
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

    public void EndGame()
    {
        PauseGame();
        disableUIInputEvent.Raise();
        unPausePlayerAnimationEvent.Raise();
        pauseGameCanvas.gameObject.SetActive(false);
        audioManager.Stop();
        var winner = GetWinner();
        var loser = GetLoser();
        PlayWinnerAnimation(winner);
        PlayLoserAnimation(loser);
        WriteWinnerInEndCanvas();
        ShowEndMenuCanvas();

    }

    private Animator GetLoser()
    {
        return new Animator();
    }

    private Animator GetWinner()
    {
        return new Animator();
    }

    private void ShowEndMenuCanvas()
    {
        endMenuCanvas.gameObject.SetActive(true);
    }

    private void WriteWinnerInEndCanvas()
    {
        
    }

    private void PlayLoserAnimation(Animator loser)
    {
        
    }

    private void PlayWinnerAnimation(Animator winner)
    {
        
    }

    private void ResetVariables()
    {
        ResetPlayerTransform();
        ResetPlayerHp();
        ResetPlayerStamina();
        ResetPlayerSpecialBar();
    }

    private void ResetPlayerSpecialBar()
    {
        
    }

    private void ResetPlayerStamina()
    {
        
    }

    private void ResetPlayerHp()
    {
        
    }

    private void ResetPlayerTransform()
    {
        
    }

    public void PauseGame()
    {
        if(isPlaying)
        {
            pauseGameCanvas.gameObject.SetActive(true);
            isPlaying = false;
            audioManager.Pause();
            fightTimer.PauseTimer();
            fightTimer.PauseCountdown();
            disablePlayerInputEvent.Raise();
            enableUIInputEvent.Raise();
            pausePlayerAnimationEvent.Raise();
        }
    }

    public void UnPauseGame()
    {
        if(!isPlaying)
        {
            pauseGameCanvas.gameObject.SetActive(false);
            isPlaying = true;
            audioManager.UnPause();
            fightTimer.StartTimer();
            fightTimer.StartCountdown();
            disableUIInputEvent.Raise();
            enablePlayerInputEvent.Raise();
            unPausePlayerAnimationEvent.Raise();
        }
    }


}
