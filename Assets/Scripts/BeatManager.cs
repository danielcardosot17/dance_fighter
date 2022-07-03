using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private Animator beatCenterAnimator;
    [SerializeField] private Animator player1BeatAnimator;
    [SerializeField] private Animator player2BeatAnimator;
    [SerializeField] private TMP_Text player1Feedback;
    [SerializeField] private TMP_Text player2Feedback;
    [SerializeField] [Range(0.0f, 0.5f)] private float beatDifferencePercentage;
    [SerializeField] private Color beatOriginalColor;
    [SerializeField] private Color beatRightColor;
    [SerializeField] private Color beatWrongColor;
    [SerializeField] private TMP_Text beatDiffText;
    private List<Animator> playerBeatAnimatorList;
    private List<bool> playersPressed;
    private List<bool> playersOnTime;
    private List<TMP_Text> playerFeedbackList;
    private List<int> playerBeatCombo;
    private float musicBpm = 0;
    private float musicLength = 0;
    private float musicStartTime = 0;
    private float pauseTime = 0;
    private float timePlayed = 0;
    private float currentBeatTime = 0;
    private float nextBeatTime = 0;

    private void Start() {
        playerBeatAnimatorList = new List<Animator>();
        playerBeatAnimatorList.Add(player1BeatAnimator);
        playerBeatAnimatorList.Add(player2BeatAnimator);

        playersPressed = new List<bool>();
        playersPressed.Add(false);
        playersPressed.Add(false);
        
        playersOnTime = new List<bool>();
        playersOnTime.Add(false);
        playersOnTime.Add(false);

        playerFeedbackList = new List<TMP_Text>();
        playerFeedbackList.Add(player1Feedback);
        playerFeedbackList.Add(player2Feedback);
        
        playerBeatCombo = new List<int>();
        playerBeatCombo.Add(0);
        playerBeatCombo.Add(0);

        ChangeColor(beatCenterAnimator.GetComponent<Image>(), beatOriginalColor);
        ChangeColor(player1BeatAnimator.GetComponent<Image>(), beatOriginalColor);
        ChangeColor(player2BeatAnimator.GetComponent<Image>(), beatOriginalColor);
    }
    


    public void StartBeatScroller(float soundBpm, float initialBeatTime, float soundLength)
    {
        musicBpm = soundBpm;
        musicLength = soundLength;
        timePlayed = 0; // rest time played
        StartBeatCenter(soundBpm, initialBeatTime, soundLength);
    }

    private void StartBeatCenter(float soundBpm, float initialBeatTime, float soundLength)
    {
        musicStartTime = Time.time;
        StartCoroutine(DoAfterTimeCoroutine(initialBeatTime,() => {
            StartCoroutine(PulseWithBpmCoroutine(soundBpm));
        }));
        StartCoroutine(DoAfterTimeCoroutine(soundLength,() => {
            PauseBeatCenter();
        }));
    }

    private IEnumerator PulseWithBpmCoroutine(float bpm)
    {
        while(true)
        {
            BeatCenterAnimation();
            MarkBeatCenterTime();
            yield return new WaitForSeconds(60/bpm);
        }
    }
    
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public void BeatCenterAnimation()
    {
        beatCenterAnimator.SetTrigger("PulseBeat");
        ChangeColor(beatCenterAnimator.GetComponent<Image>(), beatRightColor);
    }

    private void ChangeColor(Image image, Color color)
    {
        image.color = color;
    }

    public void MarkBeatCenterTime()
    {
        var diff = Time.time - currentBeatTime;
        beatDiffText.text = diff.ToString("F3");
        currentBeatTime = Time.time;
        nextBeatTime = currentBeatTime + 60/musicBpm;
        StartCoroutine(DoAfterTimeCoroutine((60/musicBpm) * beatDifferencePercentage,() => {
            CheckIfPlayersPressed();
        }));
    }

    private void CheckIfPlayersPressed()
    {
        for(int playerId = 0; playerId < 2; playerId++)
        {
            if(!playersPressed[playerId])
            {
                NotOnTimeBeat(playerId);
            }
            else
            {
                playersPressed[playerId] = false;
            }
        }
    }

    public void PlayerBeatAnimation(int playerId)
    {
        playerBeatAnimatorList[playerId].SetTrigger("PulseBeat");
    }

    public bool CheckIfPlayerBeatIsOnTime(int playerId)
    {
        playersPressed[playerId] = true;
        var playerBeatTime = Time.time;
        var currentTimeDiff = Mathf.Abs(currentBeatTime - playerBeatTime);
        var nextTimeDiff = Mathf.Abs(nextBeatTime - playerBeatTime);
        
        if((currentTimeDiff < (60/musicBpm) * beatDifferencePercentage) || (nextTimeDiff < (60/musicBpm) * beatDifferencePercentage))
        {
            OnTimeBeat(playerId);
            return true;
        }
        else
        {
            NotOnTimeBeat(playerId);
            return false;
        }
    }

    private void ChangeFeedbackText(TMP_Text tMP_Text, string text)
    {
        tMP_Text.text = text;
    }

    public void ResetPlayerBeatCombo(int playerId)
    {
        playerBeatCombo[playerId] = 0;
    }

    private void IncreaseBeatCombo(int playerId)
    {
        playerBeatCombo[playerId]++;
    }

    private void NotOnTimeBeat(int playerId)
    {
        ResetPlayerBeatCombo(playerId);

        ChangeColor(playerBeatAnimatorList[playerId].GetComponent<Image>(), beatWrongColor);
        ChangeFeedbackText(playerFeedbackList[playerId], playerBeatCombo[playerId].ToString());

        attackManager.ResetBeatCounter(playerId);
    }

    private void OnTimeBeat(int playerId)
    {
        IncreaseBeatCombo(playerId);

        ChangeColor(playerBeatAnimatorList[playerId].GetComponent<Image>(), beatRightColor);
        ChangeFeedbackText(playerFeedbackList[playerId], playerBeatCombo[playerId].ToString());
    }

    public void PauseBeatCenter()
    {
        pauseTime = Time.time;
        timePlayed += (pauseTime - musicStartTime);
        StopAllCoroutines();
    }

    public void UnPauseBeatCenter()
    {
        var beatDelay = (nextBeatTime >= pauseTime ) ? (nextBeatTime - pauseTime): (pauseTime - nextBeatTime);

        StartBeatCenter(musicBpm, beatDelay, (musicLength - timePlayed));
    }
}
