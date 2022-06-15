using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private Animator beatCenterAnimator;
    [SerializeField] private Animator player1BeatAnimator;
    [SerializeField] private Animator player2BeatAnimator;
    [SerializeField] private GameEventSO pulseBeatEvent;
    [SerializeField] [Range(0.0f, 0.1f)] private float beatDifferencePercentage;
    private List<Animator> playerBeatAnimatorList;
    private float musicBpm = 0;
    private float currentBeatTime = 0;
    private float nextBeatTime = 0;

    private void Start() {
        playerBeatAnimatorList = new List<Animator>();
        playerBeatAnimatorList.Add(player1BeatAnimator);
        playerBeatAnimatorList.Add(player2BeatAnimator);
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
            Debug.Log("Player " + playerId.ToString() + " Beat on Time!");
        }
        else
        {
            NotOnTimeBeat(playerId);
            Debug.Log("Player " + playerId.ToString() + " Beat WRONG!");
        }
    }

    private void NotOnTimeBeat(int playerId)
    {
        
    }

    private void OnTimeBeat(int playerId)
    {
        
    }
}
