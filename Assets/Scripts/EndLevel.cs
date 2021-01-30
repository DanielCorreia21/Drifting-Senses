using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            levelManager.EndLevel();
        }
    }
}
