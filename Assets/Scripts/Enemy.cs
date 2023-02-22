using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float shotCooldown = 1f;
    private bool _isOnCooldown;
    private const string BulletType = "test";
    private BulletPooler _pooler;

    // Vars for wandering

    // List for types of bullets it can shoot

    
    private void Start()
    {
        _pooler = FindObjectOfType<BulletPooler>();
    }
    
    private void FixedUpdate()
    {
        if (GameManager.Instance.playerDied) return;
        if (!GameManager.Instance.levelCompleted && Player.Instance.hasLeftSpawn)
        {
            ShootAtPlayer();
        }
    }

    private IEnumerator Cooldown()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / shotCooldown;
            yield return null;
        }
        _isOnCooldown = false;
    }

    private void ShootAtPlayer()
    {
        if (_isOnCooldown) return;
        Vector2 direction = (Player.Instance.transform.position - gameObject.transform.position).normalized;
        var rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, rotZ);
        _isOnCooldown = true;

        _pooler.SpawnFromPool(BulletType, gameObject.transform.position, rotation, direction);




        StartCoroutine(Cooldown());
    }

    // Add wandering
}
