using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    public GameObject EscPanel;
    public GameObject BackgroundMusic;
    bool Paused = false;
    bool cooldown = false;

    void Update() {
        if (Input.GetKeyDown("escape")) {
            cooldown = true;
            if (Paused) {
                Time.timeScale = 1.0f;
                EscPanel.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                BackgroundMusic.GetComponent<AudioSource>().Play();
                Paused = false;
            } else {
                Time.timeScale = 0.0f;
                EscPanel.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                BackgroundMusic.GetComponent<AudioSource>().Pause();
                Paused = true;
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1.0f;
        EscPanel.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        BackgroundMusic.GetComponent<AudioSource>().Play();
    }
}
