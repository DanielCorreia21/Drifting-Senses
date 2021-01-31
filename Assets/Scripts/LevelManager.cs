using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float lowestY = -1f;
    private Transform _playerTransform;

    public FadeText fadeText;

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
        fadeText = GameObject.FindGameObjectWithTag("FadeText")?.GetComponent<FadeText>();
        StartCoroutine(DoStartLevelText());
    }

    private IEnumerator DoStartLevelText() {

        PlayerMovement playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        GunController gunController = _playerTransform.gameObject.GetComponentInChildren<GunController>();
        playerMovement.enabled = false;
        if (gunController != null)
            gunController.enabled = false;

        StartCoroutine(fadeText.DoShowAndHideIntro(2f));
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
        if(_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
            fadeText = GameObject.FindGameObjectWithTag("FadeText")?.GetComponent<FadeText>();
        }

        if (_playerTransform != null && CheckFallOutOfLevel())
        {
            //EndGame
            //Debug.Log("Player Lost!");
            //Time.timeScale = 0;
            //maybe show ui that player lost before reseting scene
            StartCoroutine(ResetScene());
            //Time.timeScale = 1;
        }
    }

    public IEnumerator ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        while (!op.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
        fadeText = GameObject.FindGameObjectWithTag("FadeText")?.GetComponent<FadeText>();
    }

    private bool CheckFallOutOfLevel()
    {
        return _playerTransform.position.y < lowestY;
    }

    public void EndLevel()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
        PlayerMovement playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        GunController gunController = _playerTransform.gameObject.GetComponentInChildren<GunController>();
        playerMovement.StopPlayer();
        playerMovement.enabled = false;
        if (gunController != null)
            gunController.enabled = false;

        StartCoroutine(fadeText.DoShowOutro(2f));
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (op != null && !op.isDone)
        {
            yield return null;
        }
        _playerTransform = GameObject.FindGameObjectWithTag("Character")?.transform;
        fadeText = GameObject.FindGameObjectWithTag("FadeText")?.GetComponent<FadeText>();

        PlayerMovement playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        GunController gunController = _playerTransform.gameObject.GetComponentInChildren<GunController>();
        playerMovement.StopPlayer();
        playerMovement.enabled = false;
        if (gunController != null)
            gunController.enabled = false;

        StartCoroutine(fadeText.DoShowAndHideIntro(2f));
        while (fadeText.Busy)
        {
            yield return null;
        }

        playerMovement.enabled = true;
        if (gunController != null)
            gunController.enabled = true;
    }
}
