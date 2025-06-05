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
    private bool inCombat;

    [SerializeField] private AudioClip minigameMusic;
    private minigameManager minigameManager;

    //This is to apply the events
    private combatSystem combatSystem;


    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicPartInt = 0;
        combatSystem = combatSystem.instance;
        minigameManager = Singleton<minigameManager>.Instance;
        ChangeMusic(musicClips[musicPartInt].musicClip);
        inCombat = false;
    }

    //This checks for the duration of the song which once the new waiting for seconds is complete will call the change music method to move to the next loop
    IEnumerator ChangeLoopPart(int duration)
    {
        yield return new WaitForSeconds(duration);
        if(!inCombat && !minigameManager.GameInProgress)
        {
            musicPartInt++;
            if (musicPartInt >= musicClips.Length)
            {
                musicPartInt = 0;
            }
            ChangeMusic(musicClips[musicPartInt].musicClip);
        }
  
    }

    //This changes part of the song to the next part of the loop music
    public void ChangeMusic(AudioClip clip)
    {       
        musicSource.PlayOneShot(clip);
        musicSource.loop = false;
        StartCoroutine(ChangeLoopPart(musicClips[musicPartInt].musicDuration));
    }

    //This stops the board map music and plays the battle music
    public void BattleMusic(object sender, EventArgs e)
    {
        StopCoroutine(ChangeLoopPart(0));
        inCombat = true;
        musicSource.Stop();
        musicSource.clip = battleMusic;
        musicSource.Play();
        musicSource.loop = true;
        combatSystem.combatComplete += BattleMusicComplete;
    }

    //This stop the battle music and plays the board map based on where the music was left off.
    public void BattleMusicComplete(object sender, EventArgs e)
    {
        musicSource.Stop();
        ChangeMusic(musicClips[musicPartInt].musicClip);
        inCombat = false;
        combatSystem.combatComplete -= BattleMusicComplete;
    }

    public void MinigameMusic()
    {
        StopCoroutine(ChangeLoopPart(0));
        inCombat = true;
        musicSource.Stop();
        musicSource.clip = minigameMusic;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void MinigameOver(object sender, EventArgs e)
    {
        musicSource.Stop();
        ChangeMusic(musicClips[musicPartInt].musicClip);
    }

}
