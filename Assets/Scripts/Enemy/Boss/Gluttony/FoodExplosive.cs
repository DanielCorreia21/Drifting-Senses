using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodExplosive : MonoBehaviour
{
    public float speed = 20f;
    public float bulletDamage = 25f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(DestroyAfterTime());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //when we destroy, we make an explosion
        Destroy(gameObject);


        if (collision != null)
        {
            CharacterInfo character = collision.GetComponent<CharacterInfo>();
            if (character != null)
            {
                character.TakeDamage(bulletDamage);

            }

        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
