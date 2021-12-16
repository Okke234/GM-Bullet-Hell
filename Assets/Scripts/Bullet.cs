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
    //private Rigidbody2D rb;

    private void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(LifetimeCheck());
        if (boomerang)
        {
            StartCoroutine(BoomerangCheck());
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            //Debug.Log("Player has been hit!");
            Player.Instance.TakeDamage(damage);
            if (destroyOnHit)
            {
                RemoveBullet();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        RemoveBullet();
    }

    private void Move()
    {
        if (!specialPattern)
        {
            transform.position = (Vector2)transform.position + (direction * speed * Time.fixedDeltaTime);
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
        Vector3 euler = gameObject.transform.rotation.eulerAngles;
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

}
