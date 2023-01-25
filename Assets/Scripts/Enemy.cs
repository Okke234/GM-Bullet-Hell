using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float shotCooldown = 1f;
    private bool isOnCooldown = false;

    // Vars for wandering

    // List for types of bullets it can shoot


    // Start is called before the first frame update
    void Start()
    {

    }
    
    void FixedUpdate()
    {
        if (Player.Instance.hasLeftSpawn)
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
        isOnCooldown = false;
    }

    private void ShootAtPlayer()
    {
        if (!isOnCooldown)
        {
            Vector2 direction = (Player.Instance.transform.position - gameObject.transform.position).normalized;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, rot_z);
            isOnCooldown = true;

            BulletPooler.Instance.SpawnFromPool("test", gameObject.transform.position, rotation, direction);




            StartCoroutine(Cooldown());
        }
    }

    // Add wandering
}
