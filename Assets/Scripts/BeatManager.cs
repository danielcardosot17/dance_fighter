using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private Animator beatCenterAnimator;
    [SerializeField] private Animator player1BeatAnimator;
    [SerializeField] private Animator player2BeatAnimator;
    [SerializeField] private TMP_Text player1Feedback;
    [SerializeField] private TMP_Text player2Feedback;
    [SerializeField] private GameEventSO pulseBeatEvent;
    [SerializeField] [Range(0.0f, 0.1f)] private float beatDifferencePercentage;
    [SerializeField] private Color beatOriginalColor;
    [SerializeField] private Color beatRightColor;
    [SerializeField] private Color beatWrongColor;
    private List<Animator> playerBeatAnimatorList;
    private List<TMP_Text> playerFeedbackList;
    private float musicBpm = 0;
    private float currentBeatTime = 0;
    private float nextBeatTime = 0;

    private void Start() {
        playerBeatAnimatorList = new List<Animator>();
        playerBeatAnimatorList.Add(player1BeatAnimator);
        playerBeatAnimatorList.Add(player2BeatAnimator);
        playerFeedbackList = new List<TMP_Text>();
        playerFeedbackList.Add(player1Feedback);
        playerFeedbackList.Add(player2Feedback);
        ChangeColor(beatCenterAnimator.GetComponent<Image>(), beatOriginalColor);
        ChangeColor(player1BeatAnimator.GetComponent<Image>(), beatOriginalColor);
        ChangeColor(player2BeatAnimator.GetComponent<Image>(), beatOriginalColor);
    }
    


    public void StartBeatScroller(float soundBpm, float initialBeatTime, float musicLength)
    {
        StartBeatCenter(soundBpm, initialBeatTime, musicLength);
        musicBpm = soundBpm;
    }

    private void StartBeatCenter(float soundBpm, float initialBeatTime, float musicLength)
    {
        StartCoroutine(DoAfterTimeCoroutine(initialBeatTime,() => {
            StartCoroutine(PulseWithBpmCoroutine(soundBpm));
        }));
        StartCoroutine(DoAfterTimeCoroutine(musicLength,() => {
            StopAllCoroutines();
        }));
    }

    private IEnumerator PulseWithBpmCoroutine(float bpm)
    {
        while(true)
        {
            pulseBeatEvent.Raise();
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
        currentBeatTime = Time.time;
        nextBeatTime = currentBeatTime + 60/musicBpm;
    }

    public void PlayerBeatAnimation(int playerId)
    {
        playerBeatAnimatorList[playerId].SetTrigger("PulseBeat");
    }

    public void CheckIfPlayerBeatIsOnTime(int playerId)
    {
        var playerBeatTime = Time.time;
        var currentTimeDiff = Mathf.Abs(currentBeatTime - playerBeatTime);
        var nextTimeDiff = Mathf.Abs(nextBeatTime - playerBeatTime);
        
        if((currentTimeDiff < (60/musicBpm) * beatDifferencePercentage) || (nextTimeDiff < (60/musicBpm) * beatDifferencePercentage))
        {
            OnTimeBeat(playerId);
            ChangeColor(playerBeatAnimatorList[playerId].GetComponent<Image>(), beatRightColor);
            ChangeFeedbackText(playerFeedbackList[playerId], "NICE");
            Debug.Log("Player " + playerId.ToString() + " Beat on Time!");
        }
        else
        {
            NotOnTimeBeat(playerId);
            ChangeColor(playerBeatAnimatorList[playerId].GetComponent<Image>(), beatWrongColor);
            ChangeFeedbackText(playerFeedbackList[playerId], "MISS");
            Debug.Log("Player " + playerId.ToString() + " Beat WRONG!");
        }
    }

    private void ChangeFeedbackText(TMP_Text tMP_Text, string text)
    {
        tMP_Text.text = text;
    }

    private void NotOnTimeBeat(int playerId)
    {
        
    }

    private void OnTimeBeat(int playerId)
    {
        
    }
}
