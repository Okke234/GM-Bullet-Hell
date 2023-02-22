using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataManager : MonoBehaviour
{
    public BulletPooler poolerPrefab;
    public Canvas inGameOverlay;
    public List<Sprite> bulletSprites;
    public Canvas loadingScreen;
    public LevelOverScreen levelOverScreen;
    public EventSystem eventSystem;

    #region Singleton
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

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
}
