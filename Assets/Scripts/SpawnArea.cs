using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnArea : MonoBehaviour
{
    private void Start()
    {
        var bounds = GetComponent<Tilemap>().localBounds;
        if (Player.Instance == null)
        {
            Instantiate(DataManager.Instance.playerPrefab, bounds.center, new Quaternion(0, 0,  0, 0));
        }
    }
}
