using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap spawnArea;
    
    public Camera cam;
    public int speed = 5;
    public int health = 200;
    public int energy = 200;
    public bool hasLeftSpawn = false;
    private Rigidbody2D Rb { get; set; }
    

    #region Singleton
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
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

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnAreaCheck());
    }

    public void Move(float x, float y)
    {
        var movement = new Vector2(x, y);
        Rb.MovePosition(Rb.position + movement);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"Damage taken: {dmg}, Health remaining: {health}");
    }

    private IEnumerator SpawnAreaCheck()
    {
        var wait = new WaitForSeconds(0.2f);
        while (spawnArea.HasTile(Vector3Int.FloorToInt(transform.position)))
        {
            yield return wait;
        }
        hasLeftSpawn = true;
    }
}
