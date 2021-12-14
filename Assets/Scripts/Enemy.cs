using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float shotCooldown = 3f;
    private bool isOnCooldown = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShootAtPlayer();
    }

    private IEnumerator Cooldown()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / shotCooldown;
            yield return null;
        }
        isOnCooldown = false;
    }

    private void ShootAtPlayer()
    {
        if (!isOnCooldown)
        {
            Vector2 direction = (Player.Instance.transform.position - gameObject.transform.position).normalized;
            Bullet.Create(DataManager.Instance.bulletSprites[0], gameObject.transform.position, direction);
            isOnCooldown = true;
            StartCoroutine(Cooldown());
        }
    }
}
