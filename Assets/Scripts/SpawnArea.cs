using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnArea : MonoBehaviour
{
    private static Bounds _bounds;
    
    public void Start()
    {
        _bounds = GetComponent<Tilemap>().localBounds;
        Player.Instance.transform.position = _bounds.center;
        if (!Player.Instance.gameObject.activeSelf)
        {
            Player.Instance.gameObject.SetActive(true);
        }
    }

    public static void RespawnPlayer()
    {
        Player.Instance.transform.position = _bounds.center;
        Player.Instance.gameObject.SetActive(true);
    }
}
