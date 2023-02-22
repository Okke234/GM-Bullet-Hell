using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private const int StartingHealth = 200;
    private const int StartingEnergy = 200;
    private const int StartingSpeed = 10;

    [NonSerialized] public int health;
    [NonSerialized] public int energy;
    [NonSerialized] public int speed;
    [NonSerialized] public bool hasLeftSpawn;
    public static event Action OnDeath;
    public Camera cam;
    private bool _doesEndMovement = false;
    private Rigidbody2D _rb;
    private PlayerController _controller;
    

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
        
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Initialize()
    {
        health = StartingHealth;
        energy = StartingEnergy;
        speed = StartingSpeed;
        hasLeftSpawn = false;
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
        ReattachCamera();
    }
    private void OnEnable()
    {
        StartLine.OnTrigger += HandleLevelStart;
        FinishLine.OnTrigger += HandleLevelEnd;
        Initialize();
    }

    private void OnDestroy()
    {
        StartLine.OnTrigger -= HandleLevelStart;
        FinishLine.OnTrigger -= HandleLevelEnd;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.menuScene)
        {
            gameObject.SetActive(false);
        }
    }

    public void Move(float x, float y)
    {
        var movement = new Vector2(x, y);
        _rb.MovePosition(_rb.position + movement);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            cam.transform.parent = null;
            gameObject.SetActive(false);
        }
    }
    
    private void HandleLevelStart()
    {
        hasLeftSpawn = true;
    }

    public void HandleRestart() //Turn this into an event later
    {
        //Health = StartingHealth;
        hasLeftSpawn = false;
    }
    private void HandleLevelEnd()
    {
        StartCoroutine(LevelEndMovement());
        hasLeftSpawn = false;
    }

    private IEnumerator LevelEndMovement()
    {
        _doesEndMovement = true;
        yield return new WaitForSeconds(0.1f);
        _doesEndMovement = false;
    }

    private void FixedUpdate()
    {
        if (!_doesEndMovement) return;
        Move(_controller.finalInputX, _controller.finalInputY);
    }

    public void ReattachCamera()
    {
        var t = cam.transform;
        t.parent = transform;
        t.localPosition = new Vector3(0, 2, -10);
    }
}
