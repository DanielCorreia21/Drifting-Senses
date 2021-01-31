using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    
    void Awake()
    {
        StartCoroutine(RemoveExplosion());
    }

    IEnumerator RemoveExplosion()
    {
        yield return new WaitForSecondsRealtime(0.75f);

        Destroy(gameObject);
    }
}
