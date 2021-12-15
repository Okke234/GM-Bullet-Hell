using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool destroyOnHit = true;
    public bool boomerang = false;
    public bool inflictStatusEffect = false;
    public bool specialPattern = false;
    public int lifetime = -1;
    public int damage = -1;
    [Range(0,3)]
    public int speed = -1;
    public int size = -1;
    public Vector2 direction;
    public Vector2 origin;
    public Sprite sprite;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.position = (Vector2)transform.position + (direction * speed * Time.fixedDeltaTime);
    }

}
