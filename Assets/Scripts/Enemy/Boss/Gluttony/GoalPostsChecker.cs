using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPostsChecker : MonoBehaviour
{
    public GluttonyEnemy enemy;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            enemy.SetCollided();
 
        }
    }
}
