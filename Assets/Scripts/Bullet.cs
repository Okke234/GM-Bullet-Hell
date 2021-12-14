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
    private Transform origin;
    private Sprite sprite;

    private Sprite default_sprite;

    public Bullet(Sprite sprite, Transform origin, Vector2 direction, int size = 16, int speed = 5, int damage = 100, int lifetime = 5, bool specialPattern = false, bool inflictStatusEffect = false, bool boomerang = false, bool destroyOnHit = true)
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
    }

    private void Awake()
    {
        gameObject.AddComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }
}
