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
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private HealthManager healthManager;
    private List<PlayerController> playerList;
    public List<PlayerController> PlayerList { get => playerList; set => playerList = value; }

    private FightTimer fightTimer;
    private bool isPlaying = false;
    private int chosenMusicIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerList = new List<PlayerController>();
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
        if(isPlaying)
        {

        }
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

    private int ChooseRandomMusicIndex()
    {
       return UnityEngine.Random.Range(0, audioManager.GetSoundCount()); 
    }

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
        ResetPlayerAttackBar();
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
            foreach(var player in PlayerList)
            {
                player.DisableUIInput();
                player.EnablePlayerInput();
                player.UnPauseAnimation();
            }
        }
    }


}
