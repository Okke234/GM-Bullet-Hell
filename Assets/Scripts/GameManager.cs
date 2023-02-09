using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool _timerStarted;
    public float levelTimer;
    [SerializeField] private TextMeshProUGUI timerText;
    public string menuScene;
    public List<string> levels;
    [NonSerialized]
    public bool levelCompleted = false;
    // Figure out a way to get the levels as Scenes.
    private Scene _loadedLevel;
    private AsyncOperation _nextLevelLoad;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion

    private void OnEnable()
    {
        StartLine.OnTrigger += StartLevel;
        FinishLine.OnTrigger += FinishLevel;
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
        if (SceneManager.GetActiveScene().name != levels[^1])
        {
            _nextLevelLoad = SceneManager.LoadSceneAsync(levels[GetCurrentLevelIndex() + 1], LoadSceneMode.Additive);
            _nextLevelLoad.allowSceneActivation = false;
        }
        while (!_nextLevelLoad.isDone)
        {
            if (_nextLevelLoad.progress >= 0.9f)
            {
                /*
                 ON BUTTON PRESS FOR NEXT LEVEL:
                _loadedLevel.name = levels[GetCurrentLevelIndex() + 1];
                SwitchToNextLevel();
                */
            }
            else
            {
                /*
                 * 
                 * if (!_displayingLoadingScreen)
                 * {
                 *      Enable Loading screen
                 *      Update progress bar
                 *      _displayLoadingScreen = true
                 * }
                 */
                //Display loading screen when user tries to go to next level.
            }
            yield return null;
        }
    }

    private void SwitchToNextLevel()
    {
        //if (_loadedLevel.name != levels[GetCurrentLevelIndex() + 1]) return;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        _nextLevelLoad.allowSceneActivation = true;
        SceneManager.SetActiveScene(_loadedLevel);
        // This probably needs an else
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //Probably not needed...
    {
        _loadedLevel = scene;
        Debug.Log(_loadedLevel.name + " Loaded!");
    }

    private int GetCurrentLevelIndex() //Works!
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

    public void OnPlayerDeath()
    {
        Player.Instance.gameObject.SetActive(false);
    }

    private void UpdateTimer()
    {
        if (!_timerStarted) return;
        levelTimer += Time.deltaTime;
        var time = TimeSpan.FromSeconds(levelTimer);
        timerText.text = time.ToString(@"mm\:ss\:fff");
    }

    private void StartLevel()
    {
        levelTimer = 0.0f;
        _timerStarted = true;
    }

    private void FinishLevel()
    {
        levelCompleted = true;
        _timerStarted = false;
        if (GetCurrentLevelIndex() != levels.Count)
        {
            StartCoroutine(StartLoadingNextLevel());
        }
        Debug.Log("You have beaten the level!");
    }
}
