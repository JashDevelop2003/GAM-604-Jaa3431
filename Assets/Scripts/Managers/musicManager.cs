using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is using a generic singleton which makes this the only manager running in the board map
/// This will be use to change the music loop to the next file & changes the board map music to the battle music
/// </summary>

[System.Serializable]
public struct MusicClips
{
    public string musicPart;
    public AudioClip musicClip;
    public int musicDuration;
}

public class musicManager : Singleton<musicManager>
{
    private AudioSource musicSource;
    
    //This is for the music clip that plays during the board map
    [SerializeField] private MusicClips[] musicClips;
    private int musicPartInt;

    //This is for the battle music whenever combat is engaged
    [SerializeField] private AudioClip battleMusic;

    //This is to apply the events
    private combatSystem combatSystem;


    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicPartInt = 0;
        combatSystem = combatSystem.instance;
        ChangeMusic(musicClips[musicPartInt].musicClip);
    }

    //This checks for the duration of the song which once the new waiting for seconds is complete will call the change music method to move to the next loop
    IEnumerator ChangeLoopPart(int duration)
    {
        yield return new WaitForSeconds(duration);
        musicPartInt++;
        if (musicPartInt > musicClips.Length) 
        {
            musicPartInt = 0;
        }
        ChangeMusic(musicClips[musicPartInt].musicClip);
    }

    public void ChangeMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
        musicSource.loop = false;
        StartCoroutine(ChangeLoopPart(musicClips[musicPartInt].musicDuration));
    }

    public void BattleMusic(object sender, EventArgs e)
    {
        StopCoroutine(ChangeLoopPart(0));
        musicSource.PlayOneShot(battleMusic);
        musicSource.loop = true;
        combatSystem.combatComplete += BattleMusicComplete;
    }

    public void BattleMusicComplete(object sender, EventArgs e)
    {
        ChangeMusic(musicClips[musicPartInt].musicClip);
        combatSystem.combatComplete -= BattleMusicComplete;
    }

}
