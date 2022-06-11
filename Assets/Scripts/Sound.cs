using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public string category;
    public AudioClip clip;
    
    [Range(0,256)]
    public int priority;

    [Range(0f,1f)]
    public float volume;

    [Range(0.1f,3f)]
    public float pitch;

    public bool loop;
    
    public float bpm;
}
