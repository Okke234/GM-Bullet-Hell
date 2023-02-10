using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameOverlay : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    #region Singleton
    private static InGameOverlay _instance;
    public static InGameOverlay Instance => _instance;

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
    }
    #endregion
}
