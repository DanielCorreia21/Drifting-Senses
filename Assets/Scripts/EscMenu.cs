using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public GameObject EscPanel;
    public GameObject BackgroundMusic;
    bool Paused = false;

    void LateUpdate() {
        if (Input.GetKeyDown("escape")) {
            if (Paused) {
                Time.timeScale = 1.0f;
                EscPanel.gameObject.SetActive(false);
                BackgroundMusic.GetComponent<AudioSource>().Play();
                Paused = false;
            } else {
                Time.timeScale = 0.0f;
                EscPanel.gameObject.SetActive(true);
                BackgroundMusic.GetComponent<AudioSource>().Pause();
                Paused = true;
            }
        }
    }

    public void Resume() {
        Paused = false;
        Time.timeScale = 1.0f;
        EscPanel.gameObject.SetActive(false);
        BackgroundMusic.GetComponent<AudioSource>().Play();
    }

    public void Quit() {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                         Application.Quit();
        #endif
    }
}
