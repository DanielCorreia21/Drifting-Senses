using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public bool isBoss = false;
    public GameObject healthBarPrefab;

    private Transform _canvas;
    private HealthBarController _healthBar;
    private float maxHealth;
    private Transform _player;

    void Start()
    {
        if (isBoss)
        {
            _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
            maxHealth = health;
            _player = GameObject.FindGameObjectWithTag("Character").transform;
            _healthBar = null;
        }
    }

    void Update()
    {
        if(_healthBar == null && Vector3.Distance(_player.position, transform.position) < 5f)
        {
            _healthBar = Instantiate(healthBarPrefab, _canvas).GetComponent<HealthBarController>();
            Debug.Log(Vector3.Distance(_player.position, transform.position));
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (isBoss) {
            UpdateHealthBar();
        }

        if(health <= 0.001)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        Debug.Log("Updating health bar");
        float percentage = this.health / this.maxHealth;
        percentage = percentage < 0 ? 0 : percentage;
        _healthBar.UpdateHealthBar(percentage);
    }
}
