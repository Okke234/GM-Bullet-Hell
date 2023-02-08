using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    public static event Action OnTrigger;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == Player.Instance.gameObject)
        {
            OnTrigger?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
