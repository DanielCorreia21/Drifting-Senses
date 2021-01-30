using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEye : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Character")) {
            Transform views = other.transform.GetChild(other.transform.childCount-1).GetChild(1);
            for(int i = 0; i < 4; i++) {
                if (views.GetChild(i).gameObject.activeSelf) {
                    views.GetChild(i).gameObject.SetActive(false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
