using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float lowestY = -1f;
    private Transform _playerTransform;

    public FadeText fadeText;
    public string startText;
    public string endText;

    #region Singleton
    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        transform.parent = null;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion


    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
        StartCoroutine(DoStartLevelText());
    }

    private IEnumerator DoStartLevelText() {

        PlayerMovement playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        GunController gunController = _playerTransform.gameObject.GetComponent<GunController>();
        playerMovement.enabled = false;
        if (gunController != null)
            gunController.enabled = false;

        StartCoroutine(fadeText.DoShowAndHide(2f, startText));
        while (fadeText.Busy)
        {
            yield return null;
        }

        playerMovement.enabled = true;
        if (gunController != null)
            gunController.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckFallOutOfLevel())
        {
            //EndGame
            //Debug.Log("Player Lost!");
            //Time.timeScale = 0;
            //maybe show ui that player lost before reseting scene
            ResetScene();
            //Time.timeScale = 1;
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

    public void EndLevel()
    {
        PlayerMovement playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        GunController gunController = _playerTransform.gameObject.GetComponent<GunController>();
        playerMovement.StopPlayer();
        playerMovement.enabled = false;
        if (gunController != null)
            gunController.enabled = false;

        StartCoroutine(fadeText.DoShow(2f, endText));
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!op.isDone)
        {
            yield return null;
        }

        //show intro text
    }
}
