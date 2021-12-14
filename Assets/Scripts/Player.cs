using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    [SerializeField] private Camera cam;

    public int Speed { get; set; } = 5;
    public int Health { get; set; } = 200;
    public int Energy { get; set; } = 100;
    public Rigidbody2D rb { get; private set; }

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

        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float x, float y)
    {
        Vector2 movement = new Vector2(x, y);
        rb.MovePosition(rb.position + movement);
    }
}
