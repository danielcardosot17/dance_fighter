using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private List<Sound> sounds;
    private Sound currentSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        var randomIndex = ChooseRandomMusicIndex();
        Play(randomIndex);
    }

    public void Pause()
    {
        audioSource.Pause();
    }
    
    public void UnPause()
    {
        audioSource.UnPause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
    public void Play(int soundIndex)
    {
        if(sounds.Count == 0) return;
        currentSound = sounds[soundIndex];
        audioSource.clip = currentSound.clip;
        audioSource.priority = currentSound.priority;
        audioSource.volume = currentSound.volume;
        audioSource.pitch = currentSound.pitch;
        audioSource.loop = currentSound.loop;
        audioSource.Play();
    }

    public float GetCurrentSoundBpm()
    {
        return currentSound.bpm;
    }
    public float GetCurrentSoundInitialBeatTime()
    {
        return currentSound.initialBeatTime;
    }
    public float GetCurrentSoundLength()
    {
        return currentSound.loop ? 120.0f : currentSound.clip.length;
    }

    public int GetSoundCount()
    {
        return sounds.Count;
    }

    private int ChooseRandomMusicIndex()
    {
       return UnityEngine.Random.Range(0, GetSoundCount()); 
    }
}
