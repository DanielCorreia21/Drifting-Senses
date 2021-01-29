using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f;
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
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(0.3f);
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
