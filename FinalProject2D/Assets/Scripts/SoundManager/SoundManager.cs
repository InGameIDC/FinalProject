﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    public float k_BgVolume = 0.5f;
    public float k_SoundsVolume = 0.5f;
    [SerializeField] public SoundAudioClip[] soundAudioClips;
    private static SoundManager thisInstance;
    private GameObject soundGameObject;
    private AudioSource soundAudioSource;
    private AudioSource bgAudioAudioSource;

    public static SoundManager Instance
    {
        get
        {
            return thisInstance;
        }
        set { thisInstance = value; }
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        if (soundGameObject == null)
        {
            GameObject soundGameObject = new GameObject("Sound Game Object");
            soundAudioSource = soundGameObject.AddComponent<AudioSource>();
            soundAudioSource.volume = k_SoundsVolume;
        }
        // Play bgMusic on loop if it exists.
        AudioClip bgMusic = GetAudioClip(Sound.BgMusic);
        if (bgMusic != null)
        {
            bgAudioAudioSource = this.gameObject.AddComponent<AudioSource>();
            bgAudioAudioSource.loop = true;
            bgAudioAudioSource.volume = k_BgVolume;
            bgAudioAudioSource.clip = bgMusic;
            bgAudioAudioSource.Play();
        }
    }

    public void KeepPlayingBgMusic()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void PauseGame()
    {
        bgAudioAudioSource.volume = 0.1f;
        // Play Pause sound
        PlaySound(Sound.PauseClick);
    }

    public void ResumeGame()
    {
        // Play Pause sound
        PlaySound(Sound.ResumeClick);
        bgAudioAudioSource.volume = k_BgVolume;
    }

    public void DefaultButton()
    {
        PlaySound(Sound.DefaultButtonClick);
    }

    public void PlaySound(Sound soundToPlay)
    {
        AudioClip clipToPlay = GetAudioClip(soundToPlay);
        if (clipToPlay != null)
        {
            soundAudioSource.PlayOneShot(clipToPlay);
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
    BgMusic, // soundmanager
    HeroHit, // health
    HeroDie, // health
    //EnemyHit, // health
    //EnemyDie, // health
    SugarCollection, // sugar manager
    RushTime, // TimerCountdown, seconds left.
    PauseClick, // Button
    ResumeClick, // Button
    Win, // EndGame
    Lose, // EndGame
    BuyHero,
    UpgradeHero,
    StartBattle,
    PickCard,
    DefaultButtonClick,
    SelectHero,
    MoveHero
}