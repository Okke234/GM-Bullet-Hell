using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public int speed = 5;
    public int health = 200;
    public int energy = 200;
    public Rigidbody2D rb { get; private set; }

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
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float x, float y)
    {
        Vector2 movement = new Vector2(x, y);
        rb.MovePosition(rb.position + movement);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"Damage taken: {dmg}, Health remaining: {health}");
    }
}
