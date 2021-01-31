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
        GameObject playerOj = GameObject.FindGameObjectWithTag("Character");
        rb.velocity = (playerOj.transform.position - transform.position) * speed;
        StartCoroutine(DestroyAfterTime());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //when we destroy, we make an explosion
        //We show explosion on screeen, and do a sphereCast to see if player is inside
        //Or check distance between this and the player, and check if less than blast radius
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
