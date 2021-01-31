using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickupController : MonoBehaviour
{
    public float regenAmmount = 5f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            CharacterInfo charInfo = other.gameObject.GetComponent<CharacterInfo>();
            if(charInfo != null)
            {
                charInfo.RegenHealth(regenAmmount);
                Destroy(gameObject);
            }
        }
    }
}
