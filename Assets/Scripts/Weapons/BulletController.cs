using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f;
    public float bulletDamage = 10f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(DestroyAfterTime());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if(collision != null)
        {
            EnemyInfo enemy = collision.GetComponent<EnemyInfo>();
            if(enemy != null)
            {
                enemy.TakeDamage(bulletDamage);

            }

        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
