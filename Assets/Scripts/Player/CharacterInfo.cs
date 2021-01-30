using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{

    public float health = 100f;
    public GameObject healthBarPrefab;

    private float maxHealth;
    private HealthBarController _healthBar;

    private void Start()
    {
        maxHealth = health;
        _healthBar = Instantiate(healthBarPrefab, GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<HealthBarController>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        float percentage = health / maxHealth;
        percentage = percentage < 0 ? 0 : percentage;
        _healthBar.UpdateHealthBar(percentage);

        if(health <= 0)
        {
            StartCoroutine(LevelManager.Instance.ResetScene());
        }
        //TODO death
    }
}
