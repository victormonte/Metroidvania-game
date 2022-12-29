using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRb;

    public Vector2 moveDir;

    public GameObject impactEffect;

    public int damageAmount = 1;

    // Update is called once per frame
    void Update()
    {
        theRb.velocity = moveDir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            // damage the enemy
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (other.tag == "Boss")
        {
            BossHealthController.instance.TakeDamage(damageAmount);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
