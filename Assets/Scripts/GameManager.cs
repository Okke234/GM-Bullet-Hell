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
    private Scene loadedLevel;
    private AsyncOperation nextLevelLoad;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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
        if (SceneManager.GetActiveScene() != levels[levels.Count - 1])
        {
            nextLevelLoad = SceneManager.LoadSceneAsync(levels[GetCurrentLevelIndex() + 1].name, LoadSceneMode.Additive);
            nextLevelLoad.allowSceneActivation = false;
        }
        while (!nextLevelLoad.isDone)
        {
            yield return null;
        }
    }

    public void SwitchToNextLevel()
    {
        if (loadedLevel != null && loadedLevel == levels[GetCurrentLevelIndex() + 1])
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.SetActiveScene(loadedLevel);
        }
        // This probably needs an else
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuScene.name, LoadSceneMode.Single);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedLevel = scene;
    }

    private int GetCurrentLevelIndex()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] == SceneManager.GetActiveScene())
            {
                return i;
            }
        }
        return -1;
    }

}
