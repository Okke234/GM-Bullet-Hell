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
    [Range(1, 10)]
    public int lifetime = -1;
    [Range(1, 300)]
    public int damage = -1;
    [Range(0, 3)]
    public int speed = -1;
    public int size = -1;
    public Vector2 direction;
    public Vector2 origin;
    public Sprite sprite;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(LifetimeCheck());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == Player.Instance.gameObject)
        {
            //Debug.Log("Player has been hit!");
            RemoveBullet();
            Player.Instance.TakeDamage(damage);
        }
    }

    private void Move()
    {
        transform.position = (Vector2)transform.position + (direction * speed * Time.fixedDeltaTime);
    }

    private void RemoveBullet()
    {
        gameObject.SetActive(false);
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

}
