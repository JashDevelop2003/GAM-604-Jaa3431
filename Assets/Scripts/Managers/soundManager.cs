using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is using a generic singleton which makes this the only manager running in the board map
/// This will be use to change the music loop to the next file & changes the board map music to the battle music
/// </summary>

public class soundManager : Singleton<soundManager>
{
    private AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
