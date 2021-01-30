﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] public SoundAudioClip[] sounds;

    public static SoundManager Instance { get; private set; }

    void Awake() {
        transform.parent = null;
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    public enum Sound {
        PlayerBullet,
        PlayerDash,
        MedusaDead,
        MedusaLaser,
        MedusaHurt
    }

    public void PlaySound(Sound sound, float soundVolume) {
        GameObject soundgameobject = new GameObject("Sound");
        AudioSource audioSource = soundgameobject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), soundVolume);
    }

    private AudioClip GetAudioClip(Sound sound) {
        foreach(SoundAudioClip s in sounds){
            if(s.sound == sound) {
                return s.audioClip;
            }
        }
        return null;
    }
        

    [System.Serializable]
    public class SoundAudioClip{
        public Sound sound;
        public AudioClip audioClip;
    }

}
