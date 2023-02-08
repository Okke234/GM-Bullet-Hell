using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap spawnArea;

    public UnityEvent onDeath;
    public Camera cam;
    public int speed = 5;
    public int health = 200;
    public int energy = 200;
    public bool hasLeftSpawn = false;

    private bool _doesEndMovement = false;
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

    private void OnEnable()
    {
        StartLine.OnTrigger += HandleLevelStart;
        FinishLine.OnTrigger += HandleLevelEnd;
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
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
        if (health <= 0) onDeath?.Invoke();
    }
    
    private void HandleLevelStart()
    {
        hasLeftSpawn = true;
    }
    
    private void HandleLevelEnd()
    {
        StartCoroutine(LevelEndMovement());
    }

    private IEnumerator LevelEndMovement()
    {
        _doesEndMovement = true;
        yield return new WaitForSeconds(2);
        _doesEndMovement = false;
    }

    private void FixedUpdate()
    {
        if (!_doesEndMovement) return;
        Move(PlayerController.finalInputX, PlayerController.finalInputY);
    }
}
