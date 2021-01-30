using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject controlsMenu;
    public GameObject mainMenu;

    private void Start()
    {
        controlsMenu.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ControlsButton()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void BackButton()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
