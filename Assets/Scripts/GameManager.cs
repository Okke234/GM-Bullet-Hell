using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string menuScene;
    public List<string> levels;
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

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion

    public void LoadLevelFromMenu(string level)
    {
        if (SceneManager.GetActiveScene().name != level)
        {
            SceneManager.LoadScene(level);
        }
    }

    public IEnumerator StartLoadingNextLevel()
    {
        if (SceneManager.GetActiveScene().name != levels[^1])
        {
            _nextLevelLoad = SceneManager.LoadSceneAsync(levels[GetCurrentLevelIndex() + 1], LoadSceneMode.Additive);
            _nextLevelLoad.allowSceneActivation = false;
        }
        while (!_nextLevelLoad.isDone)
        {
            yield return null;
        }
    }

    public void SwitchToNextLevel()
    {
        if (_loadedLevel.name != levels[GetCurrentLevelIndex() + 1]) return;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
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


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _loadedLevel = scene;
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

}
