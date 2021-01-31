using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodExplosive : MonoBehaviour
{
    public float speed = 20f;
    public float bulletDamage = 25f;
    public Rigidbody2D rb;
    public GameObject explosionPrefab;
    public float explosionRadius = 1f;

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

        //ignore bullets
        if(collision.GetComponent<BulletController>() != null) { 
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, LevelManager.Instance.transform);
        explosion.transform.position = gameObject.transform.position;
        Debug.Log("Explosion");


        if (collision != null)
        {
            GameObject characterObj = GameObject.FindGameObjectWithTag("Character");
            if(Vector2.Distance(characterObj.transform.position, transform.position) < this.explosionRadius)
            {
                CharacterInfo characterInfo = characterObj.GetComponent<CharacterInfo>();
                characterInfo.TakeDamage(bulletDamage);
            }

        }
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
