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
    private List<Animator> playerBeatAnimatorList;

    private void Start() {
        playerBeatAnimatorList = new List<Animator>();
        playerBeatAnimatorList.Add(player1BeatAnimator);
        playerBeatAnimatorList.Add(player2BeatAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBeatScroller(float soundBpm, float initialBeatTime, float musicLength)
    {
        StartBeatCenter(soundBpm, initialBeatTime, musicLength);
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

    public void PlayerBeatAnimation(int playerId)
    {
        playerBeatAnimatorList[playerId].SetTrigger("PulseBeat");
        Debug.Log("Player " + playerId.ToString() + " Beat");
    }
}
