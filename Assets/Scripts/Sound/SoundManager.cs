using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Player Sounds
    [SerializeField] AudioClip bullet;
    [SerializeField] AudioClip dash;


    //Background music
    [SerializeField] AudioClip solitude;
    [SerializeField] AudioClip vision;

    //Player
    [SerializeField] AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayBullet() {
        playerAudio.PlayOneShot(bullet);
    }
    public void PlayDash() {
        playerAudio.PlayOneShot(dash);
    }

}
