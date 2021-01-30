using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float lowestY = -1f;
    private Transform _playerTransform;
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckFallOutOfLevel())
        {
            //EndGame
            //Debug.Log("Player Lost!");
            ResetScene();
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool CheckFallOutOfLevel()
    {
        return _playerTransform.position.y < lowestY;
    }
}
