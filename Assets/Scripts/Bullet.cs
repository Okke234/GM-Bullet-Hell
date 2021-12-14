using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool destroyOnHit = true;
    private bool boomerang = false;
    private bool inflictStatusEffect = false;
    private bool specialPattern = false;
    private int lifetime = -1;
    private int damage = -1;
    private int speed = -1;
    private int size = -1;
    private Vector2 direction;
    private Vector2 origin;
    private Sprite sprite;

    /*public Bullet(Sprite sprite, Vector2 origin, Vector2 direction, int size = 16, int speed = 5, int damage = 100, int lifetime = 5, bool specialPattern = false, bool inflictStatusEffect = false, bool boomerang = false, bool destroyOnHit = true)
    {
        this.direction = direction;
        this.sprite = sprite;
        this.origin = origin;
        this.size = size;
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.specialPattern = specialPattern;
        this.inflictStatusEffect = inflictStatusEffect;
        this.boomerang = boomerang;
        this.destroyOnHit = destroyOnHit;
    }*/

    public static Bullet Attach(GameObject go, Sprite sprite, Vector2 origin, Vector2 direction, int size = 16, int speed = 5, int damage = 100, int lifetime = 5, bool specialPattern = false, bool inflictStatusEffect = false, bool boomerang = false, bool destroyOnHit = true)
    {
        Bullet bullet = go.AddComponent<Bullet>();
        bullet.gameObject.AddComponent<Rigidbody2D>();
        bullet.gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

        return bullet;
    }

    public static Bullet Create(Sprite sprite, Vector2 origin, Vector2 direction, int size = 16, int speed = 5, int damage = 100, int lifetime = 5, bool specialPattern = false, bool inflictStatusEffect = false, bool boomerang = false, bool destroyOnHit = true)
    {
        return Attach(new GameObject("Bullet"), sprite, origin, direction, size, speed, damage, lifetime, specialPattern, inflictStatusEffect, boomerang, destroyOnHit);
    }

    /*private void Awake()
    {
        gameObject.AddComponent<Rigidbody2D>();
        SpriteRenderer r = gameObject.AddComponent<SpriteRenderer>();
        r.sprite = sprite;
    }*/

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Don't do this...
        var rb = gameObject.GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + direction);
    }
}
