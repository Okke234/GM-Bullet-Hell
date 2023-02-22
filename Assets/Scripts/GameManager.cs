using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public string menuScene;
    [SerializeField] private List<string> levels;
    [NonSerialized] public bool levelCompleted = false;
    [NonSerialized] public bool playerDied = false;
    
    private float _levelTimer;
    private bool _timerStarted;
    private TextMeshProUGUI _timerText;
    
    private Scene _loadedLevel;
    private AsyncOperation _nextLevelLoad;

    private Canvas _loadingScreen;
    private TextMeshProUGUI _loadingProgress;
    private bool _displayingLoadingScreen;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    private void OnEnable()
    {
        StartLine.OnTrigger += StartLevel;
        FinishLine.OnTrigger += FinishLevel;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Player.OnDeath += OnPlayerDeath;
    }
    
    private void OnDestroy()
    {
        StartLine.OnTrigger -= StartLevel;
        FinishLine.OnTrigger -= FinishLevel;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Player.OnDeath -= OnPlayerDeath;
    }

    private void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = Instantiate(DataManager.Instance.eventSystem);
            DontDestroyOnLoad(eventSystem);
        }
    }

    private void Update()
    {
        UpdateTimer();
    }
    
    public void LoadLevelFromMenu(string level)
    {
        if (SceneManager.GetActiveScene().name != level)
        {
            SceneManager.LoadScene(level);
        }
    }

    private IEnumerator StartLoadingNextLevel()
    {
        var displayingContinueText = false;
        if (SceneManager.GetActiveScene().name != levels[^1])
        {
            _nextLevelLoad = SceneManager.LoadSceneAsync(levels[GetCurrentLevelIndex() + 1], LoadSceneMode.Single);
            _nextLevelLoad.allowSceneActivation = false;
        }
        while (!_nextLevelLoad.isDone)
        {
            if (_nextLevelLoad.progress >= 0.9f)
            {
                if (_displayingLoadingScreen && !displayingContinueText)
                {
                    _loadingProgress.text = "Press Space to continue...";
                    displayingContinueText = true;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var nextLevelName = levels[GetCurrentLevelIndex() + 1];
                    SwitchToNextLevel(nextLevelName);
                    _displayingLoadingScreen = false;
                    Destroy(_loadingScreen);
                    //yield return null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //ONLY WHEN THE PLAYER HAS CHOSEN TO SWITCH LEVEL BEFORE IT HAS FINISHED LOADING!!   
                 if (!_displayingLoadingScreen)
                 {
                     DisplayLoadingScreen();
                     _displayingLoadingScreen = true;
                 }

                 _loadingProgress.text = "Loading progress: " + (_nextLevelLoad.progress * 100) + "%";
            }
            yield return null;
        }
    }

    private void SwitchToNextLevel(string nextLevel)
    {
        //if (_loadedLevel.name != levels[GetCurrentLevelIndex() + 1]) return;
        _nextLevelLoad.allowSceneActivation = true;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextLevel));
        //SceneManager.UnloadSceneAsync(_loadedLevel);
        // This probably needs an else
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SpawnArea.RespawnPlayer();
    }

    public void ReturnToMenu()
    {
        Player.Instance.ReattachCamera();
        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == menuScene) return;
        Player.Instance.HandleRestart();
        playerDied = false;
        levelCompleted = false;
        _loadedLevel = scene;
        MissingObjectsCheck();
        //Debug.Log(_loadedLevel.name + " Loaded!");
    }

    private int GetCurrentLevelIndex()
    {
        for (var i = 0; i < levels.Count; i++)
        {
            if (levels[i] == SceneManager.GetActiveScene().name)
            {
                return i;
            }
        }
        return -1;
    }

    private void DisplayLoadingScreen()
    {
        _loadingScreen = DataManager.Instance.loadingScreen;
        _loadingProgress = _loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
        _loadingScreen = Instantiate(_loadingScreen);
        Debug.Log("Displaying loading screen!");
    }

    public void OnPlayerDeath()
    {
        //Player.Instance.gameObject.SetActive(false);
        //Time.timeScale = 0;
        playerDied = true;
        _timerStarted = false;
        var los = Instantiate(DataManager.Instance.levelOverScreen);
        los.deathScreen.SetActive(true);
        var time = TimeSpan.FromSeconds(_levelTimer);
        los.endTimer.text = "You died in: " + time.ToString(@"mm\:ss\:fff");
    }

    private void UpdateTimer()
    {
        if (!_timerStarted) return;
        _levelTimer += Time.deltaTime;
        var time = TimeSpan.FromSeconds(_levelTimer);
        _timerText.text = time.ToString(@"mm\:ss\:fff");
    }

    private void StartLevel()
    {
        _levelTimer = 0.0f;
        _timerStarted = true;
    }

    private void FinishLevel()
    {
        var los = Instantiate(DataManager.Instance.levelOverScreen);
        if (SceneManager.GetActiveScene().name != levels[^1])
        {
            los.completedScreen.SetActive(true);
        }
        else
        {
            los.finalScreen.SetActive(true);
        }
        var time = TimeSpan.FromSeconds(_levelTimer);
        los.endTimer.text = "You finished in: " + time.ToString(@"mm\:ss\:fff");
        levelCompleted = true;
        _timerStarted = false;
        if (GetCurrentLevelIndex() != levels.Count-1)
        {
            StartCoroutine(StartLoadingNextLevel());
        }
    }

    private void MissingObjectsCheck()
    {
        if (InGameOverlay.Instance == null)
        {
            Instantiate(DataManager.Instance.inGameOverlay);
            _timerText = InGameOverlay.Instance.timerText;
        }
    }
}
