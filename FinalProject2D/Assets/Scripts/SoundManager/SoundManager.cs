using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public SoundAudioClip[] soundAudioClips;
    private static SoundManager thisInstance;

    public static SoundManager Instance
    {
        get
        {
            if (thisInstance == null)
            {
                thisInstance = GameObject.FindObjectOfType<SoundManager>();
            }

            // If it's still null - there is no sound manager.
            if (thisInstance == null)
            {
                Debug.Log("There is no sound manager!");
            }

            return thisInstance;
        }
    }

    public void Awake()
    {
        // Play bgMusic on loop if it exists.
        AudioClip bgMusic = GetAudioClip(Sound.BgMusic);
        if (bgMusic != null)
        {
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.PlayOneShot(bgMusic);
        }
    }

    public void PlaySound(Sound soundToPlay)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioClip clipToPlay = GetAudioClip(soundToPlay);
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clip selected");
        }
    }

    private AudioClip GetAudioClip(Sound soundToPlay)
    {
        AudioClip result = null;
        List<AudioClip> allPossibleCLips = new List<AudioClip>();
        foreach (SoundAudioClip soundAudioClip in soundAudioClips)
        {
            if (soundAudioClip.sound == soundToPlay)
            {
                allPossibleCLips.Add(soundAudioClip.audioClip);
            }
        }

        // Adds randomness if several audio clips were attached.
        if (allPossibleCLips.Count > 0)
        {
            Random random = new Random();
            int index = random.Next(0, allPossibleCLips.Count);
            result = allPossibleCLips[index];
        }
        return result;
    }
}

[System.Serializable]
public class SoundAudioClip
{
    public Sound sound;
    public AudioClip audioClip;
}
public enum Sound
{
    BgMusic,
    HeroHit,
    HeroDie,
    EnemyHit,
    EnemyDie,
    SugarCollection,
    RushTime,
    PauseClick,
    ResumeClick,
    Win,
    Lose
}