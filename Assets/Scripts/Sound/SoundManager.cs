using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip bullet;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip solitude;
    [SerializeField] AudioClip vision;
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
