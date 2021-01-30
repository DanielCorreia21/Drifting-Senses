using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public bool isBoss = false;
    public GameObject healthBarPrefab;
    public LineRenderer laser;
    public Transform firingPoint;

    private Transform _canvas;
    private HealthBarController _healthBar;
    private float maxHealth;
    private Transform _player;
    private int layerMask;


    void Start()
    {
        if (isBoss)
        {
            _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
            maxHealth = health;
            _player = GameObject.FindGameObjectWithTag("Character").transform;
            _healthBar = null;
        }
        layerMask = ~LayerMask.GetMask("Enemy");
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


    public void Attack()
    {
        //Debug.Log("Preparing attack!");
        StartCoroutine(MedusaAttack());
    }

    IEnumerator MedusaAttack()
    {
        //Debug.Log("Preparing Attack: called at: " + Time.time);
        Vector3 playerPos = _player.position;
        yield return new WaitForSecondsRealtime(0.3f);

        laser.SetPosition(0,firingPoint.position);
        RaycastHit2D hitInfo = Physics2D.Raycast(firingPoint.position, (playerPos - firingPoint.position), 10, layerMask);
        if (hitInfo)
        {
            laser.SetPosition(1, hitInfo.point);
            CharacterInfo playerInfo = hitInfo.transform.GetComponent<CharacterInfo>();
            if(playerInfo != null)
            {
                //TODO damage player
            }
        }
        else
        {
            Vector3 direction = (playerPos - firingPoint.position);
            direction *= 1.5f;
            laser.SetPosition(1, firingPoint.position + direction);
        }

        yield return new WaitForSecondsRealtime(0.3f);
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
        //Debug.Log("Attacking: called at: " + Time.time);
    }
}
