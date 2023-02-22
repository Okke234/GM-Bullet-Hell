using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public bool destroyOnHit = true;
    public bool boomerang = false;
    public bool inflictStatusEffect = false;
    public bool specialPattern = false;
    public int patternId = -1;
    [Range(1, 10)]
    public int lifetime = -1;
    [Range(1, 300)]
    public int damage = -1;
    [Range(0, 10)]
    public int speed = -1;
    public int size = -1;
    public Vector2 direction;
    public Vector2 origin;
    public Sprite sprite;

    private bool _hasDealtDamage = false;
    private bool _hasLevelStarted = false;
    private SpriteRenderer _spriteRenderer;
    //private Rigidbody2D rb;

    private void OnEnable()
    {
        StartLine.OnTrigger += HandleLevelStart;
        FinishLine.OnTrigger += HandleLevelEnd;
        Player.OnDeath += OnPlayerDeath;
    }
    
    private void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LifetimeCheck());
        if (boomerang)
        {
            StartCoroutine(BoomerangCheck());
        }
    }

    private void FixedUpdate()
    {
        if (_hasLevelStarted)
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.playerDied || GameManager.Instance.levelCompleted) return;
        if (other.gameObject != Player.Instance.gameObject) return;
        if (_hasDealtDamage)
        {
            return;
        }
        //Debug.Log("Player has been hit!");
        Player.Instance.TakeDamage(damage);
        _hasDealtDamage = true;
        if (destroyOnHit)
        {
            RemoveBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.playerDied && !GameManager.Instance.levelCompleted)
        {
            if (collision.gameObject == Player.Instance.gameObject)
            {
                return;
            }
        }

        RemoveBullet();
    }

    private void Move()
    {
        if (!specialPattern)
        {
            var t = transform;
            t.position = (Vector2)t.position + (direction * (speed * Time.fixedDeltaTime));
        } /* else {
           * MoveInPattern(patternId);
           * }*/
    }

    private void RemoveBullet()
    {
        gameObject.SetActive(false);
    }

    private void FlipBullet()
    {
        direction *= -1;
        var euler = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.Rotate(new Vector3(euler.x, euler.y, 180f));
    }


    // If pool size is too small bullets can be repurposed while the original check is still running
    private IEnumerator LifetimeCheck()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / lifetime;
            yield return null;
        }
        RemoveBullet();
    }

    private IEnumerator BoomerangCheck()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 0.5f)
        {
            normalizedTime += Time.deltaTime / lifetime;
            yield return null;
        }
        FlipBullet();
        _hasDealtDamage = false;
    }

    /*private void MoveInPattern(int id) {
     * switch (id)
     * {
     * case 1:
     * transform.position = (Vector2)transform.position + Mathf.Cos(direction * speed * Time.fixedDeltaTime);
     * 
     * case 2:
     * return null;
     * }
     * }
     */

    private void HandleLevelStart()
    {
        _hasLevelStarted = true;
    }

    private void HandleLevelEnd()
    {
        Unsubscribe();
        _hasLevelStarted = false;
        if (gameObject.activeSelf && _spriteRenderer != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private void OnPlayerDeath()
    {
        Unsubscribe();
        if (gameObject.activeSelf && _spriteRenderer != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        var alpha = _spriteRenderer.color.a;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / 3.0f)
        {
            _spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(alpha, 0.0f, t));
            yield return null;
        }
    }

    private void Unsubscribe()
    {
        StartLine.OnTrigger -= HandleLevelStart;
        FinishLine.OnTrigger -= HandleLevelEnd;
        Player.OnDeath -= OnPlayerDeath;
    }
}
