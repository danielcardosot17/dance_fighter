using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Canvas startMenuCanvas;
    [SerializeField] private Canvas endMenuCanvas;
    [SerializeField] private Canvas pauseGameCanvas;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private TMP_Text endMenuText;
    [SerializeField] private int numberOfVictoryAnimations;
    [SerializeField] private int numberOfDefeatAnimations;
    [SerializeField] private int numberOfAttacksLightAnimations;
    [SerializeField] private int numberOfAttacksMediumAnimations;
    [SerializeField] private int numberOfAttacksHeavyAnimations;
    private List<PlayerController> playerList;
    public List<PlayerController> PlayerList { get => playerList; set => playerList = value; }
    public int NumberOfVictoryAnimations { get => numberOfVictoryAnimations; set => numberOfVictoryAnimations = value; }
    public int NumberOfDefeatAnimations { get => numberOfDefeatAnimations; set => numberOfDefeatAnimations = value; }
    public int NumberOfAttacksLightAnimations { get => numberOfAttacksLightAnimations; set => numberOfAttacksLightAnimations = value; }
    public int NumberOfAttacksMediumAnimations { get => numberOfAttacksMediumAnimations; set => numberOfAttacksMediumAnimations = value; }
    public int NumberOfAttacksHeavyAnimations { get => numberOfAttacksHeavyAnimations; set => numberOfAttacksHeavyAnimations = value; }

    private FightTimer fightTimer;
    private bool isPlaying = false;
    private int chosenMusicIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        PlayerList = new List<PlayerController>();
        fightTimer = GetComponent<FightTimer>();
        startMenuCanvas.gameObject.SetActive(true);
        endMenuCanvas.gameObject.SetActive(false);
        pauseGameCanvas.gameObject.SetActive(false);
        playerSpawner.PreviewModel(0,0);
        playerSpawner.PreviewModel(1,0);
    }

    // Update is called once per frame
    void Update()
    {
        // if(isPlaying)
        // {

        // }
    }

    public void ChooseMusic(int musicIndex)
    {
        chosenMusicIndex = musicIndex;
    }

    public void StartGame()
    {
        playerSpawner.DestroyAllPreviewModels();
        playerSpawner.DestroyPlayers();
        playerSpawner.SpawnPlayers();
        PlayerList = playerSpawner.GetPlayerControllerList();
        isPlaying = true;
        startMenuCanvas.gameObject.SetActive(false);
        endMenuCanvas.gameObject.SetActive(false);
        ResetVariables();
        StartMusic(chosenMusicIndex);
        StartBeatScroller(audioManager.GetCurrentSoundBpm(), audioManager.GetCurrentSoundInitialBeatTime(),audioManager.GetCurrentSoundLength());
        SetTimerLength(audioManager.GetCurrentSoundLength());
        ResetCountdownTimer();
        StartTimer();
        StartCountdown();
    }

    private void StartCountdown()
    {
        fightTimer.StartCountdown();
    }

    public void StartPlayers()
    {
        foreach(var player in PlayerList)
        {
            player.DisableUIInput();
            player.EnablePlayerInput();
        }
    }

    private void SetTimerLength(float length)
    {
        fightTimer.SetTimerLength(length);
    }

    private void ResetCountdownTimer()
    {
        fightTimer.ResetCountdownTimer();
    }

    public void StartMusic(int musicIndex)
    {
        audioManager.Stop();
        audioManager.Play(musicIndex);
    }

    private void StartBeatScroller(float soundBpm, float initialBeatTime, float musicLength)
    {
        beatManager.StartBeatScroller(soundBpm, initialBeatTime, musicLength);
    }

    private void StartTimer()
    {
        fightTimer.StartTimer();
    }

    // private int ChooseRandomMusicIndex()
    // {
    //    return UnityEngine.Random.Range(0, audioManager.GetSoundCount()); 
    // }

    public void EndGame()
    {
        PauseGame();
        foreach(var player in PlayerList)
        {
            player.DisableUIInput();
            player.UnPauseAnimation();
        }
        pauseGameCanvas.gameObject.SetActive(false);
        audioManager.Stop();

        if(healthManager.IsTie())
        {
            WriteTieInCanvas();
        }
        else
        {
            var winnerId = healthManager.GetWinnerId();
            var loserId = healthManager.GetLoserId();
            PlayWinnerAnimation(winnerId);
            PlayLoserAnimation(loserId);
            WriteWinnerInEndCanvas(winnerId);
        }
        ShowEndMenuCanvas();
    }

    private void WriteTieInCanvas()
    {
        endMenuText.text = "Tie!";
    }

    private void ShowEndMenuCanvas()
    {
        endMenuCanvas.gameObject.SetActive(true);
    }

    private void WriteWinnerInEndCanvas(int winnerId)
    {
        endMenuText.text = "Player " + (winnerId+1).ToString() + "\n Victory!";
    }

    private void PlayLoserAnimation(int loserId)
    {
        PlayerList[loserId].PlayRandomLoserAnimation();
    }

    private void PlayWinnerAnimation(int winnerId)
    {
        PlayerList[winnerId].PlayRandomWinnerAnimation();
    }

    private void ResetVariables()
    {
        ResetPlayerTransform();
        ResetPlayerHp();
        ResetPlayerAttackBar();
        ResetPlayerBeatCombo();
    }

    private void ResetPlayerBeatCombo()
    {
        foreach(var player in PlayerList)
        {
            beatManager.ResetPlayerBeatCombo(player.PlayerId);
        }
    }

    private void ResetPlayerAttackBar()
    {
        foreach(var player in PlayerList)
        {
            attackManager.ResetBeatCounter(player.PlayerId);
        }
    }

    private void ResetPlayerHp()
    {
        foreach(var player in PlayerList)
        {
            healthManager.ResetPlayerHp(player.PlayerId);
        }
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
            beatManager.PauseBeatCenter();
            foreach(var player in PlayerList)
            {
                player.DisablePlayerInput();
                player.EnableUIInput();
                player.PauseAnimation();
            }
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
            beatManager.UnPauseBeatCenter();
            foreach(var player in PlayerList)
            {
                player.DisableUIInput();
                player.EnablePlayerInput();
                player.UnPauseAnimation();
            }
        }
    }


}
