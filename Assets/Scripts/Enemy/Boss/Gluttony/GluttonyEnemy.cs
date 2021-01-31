﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyEnemy : EnemyInfo
{
    public float health = 1500f;
    public bool isBoss = false;
    public Transform firingPoint;
    public Rigidbody2D rb;
    public GameObject gluttonySuperParticleEffect;
    public GameObject foodPrefab;
    public float speed = 2f;
    private Stack<float> _superPercentages;


    public LineRenderer laser;
    private Transform _canvas;
    public GameObject healthBarPrefab;
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
            _superPercentages = new Stack<float>();
            _superPercentages.Push(health * 0.25f);
            _superPercentages.Push(health * 0.50f);
            _superPercentages.Push(health * 0.75f);
        }
        layerMask = ~LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if(_healthBar == null && Vector3.Distance(_player.position, transform.position) < 5f)
        {
            _healthBar = Instantiate(healthBarPrefab, _canvas).GetComponent<HealthBarController>();
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;

        if (isBoss) {
            UpdateHealthBar();
        }

        if(health <= 0.001)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MedusaDead, 1f);
        transform.GetComponent<Animator>().SetTrigger("Died");
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.EndLevel();
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        float percentage = this.health / this.maxHealth;
        percentage = percentage < 0 ? 0 : percentage;
        _healthBar.UpdateHealthBar(percentage);
        SoundManager.Instance.PlaySound(SoundManager.Sound.MedusaHurt, 1f);
    }


    public void Attack()
    {
        //Debug.Log("Preparing attack!");
        StartCoroutine(MedusaAttack());
    }

    public void SuperAttack()
    {
        StartCoroutine(GluttonySuperAttack());
    }

    IEnumerator GluttonySuperAttack()
    {

        gluttonySuperParticleEffect.SetActive(true);
        Debug.Log("Enabling particle effect");
        yield return new WaitForSeconds(1.08f);
        gluttonySuperParticleEffect.SetActive(false);

        Coroutine movedCor = StartCoroutine(CheckPlayerMoved(_player.position));

        yield return new WaitForSeconds(1f);

        StopCoroutine(movedCor);
        
    }

    IEnumerator CheckPlayerMoved(Vector3 initialPos)
    {
        Vector3 newPos = _player.position;
        while (Vector3.Distance(initialPos, newPos) < 0.01)
        {
            newPos = _player.position;
            yield return null;
        }
        _player.GetComponent<CharacterInfo>()?.TakeDamage(50f);
    }

    IEnumerator MedusaAttack()
    {
        //Debug.Log("Preparing Attack: called at: " + Time.time);
        Vector3 playerPos = _player.position;
        yield return new WaitForSecondsRealtime(0.3f);

        laser.SetPosition(0,firingPoint.position);
        RaycastHit2D hitInfo = Physics2D.Raycast(firingPoint.position, (playerPos - firingPoint.position), 20, layerMask);
        if (hitInfo)
        {
            laser.SetPosition(1, hitInfo.point);
            CharacterInfo playerInfo = hitInfo.transform.GetComponent<CharacterInfo>();
            if(playerInfo != null)
            {
                //TODO damage player
                playerInfo.TakeDamage(10f);
            }
        }
        else
        {
            Vector3 direction = (playerPos - firingPoint.position);
            direction *= 1.5f;
            laser.SetPosition(1, firingPoint.position + direction);
        }
        SoundManager.Instance.PlaySound(SoundManager.Sound.MedusaLaser, 0.3f);

        yield return new WaitForSecondsRealtime(0.15f);
        laser.SetPosition(0, Vector3.zero);
        laser.SetPosition(1, Vector3.zero);
        //Debug.Log("Attacking: called at: " + Time.time);
    }

    internal void TriggerAttack(Animator animator)
    {
        if(_superPercentages.Count > 0 && health < _superPercentages.Peek())
        {
            _superPercentages.Pop();
            Debug.Log("Super");
            animator.SetTrigger("Super");
        }
        else
        {
            animator.SetTrigger("Attack");
        }

    }

    private bool onRush = false;
 
    public bool collided = false;

    internal void TriggerBullRush(Animator animator)
    {
        int mult = 1;
        float targetX = _player.position.x;
        if (targetX < transform.position.x)
        {
            mult = -1;
        }

        StartCoroutine(BullRush(mult,animator));

    }


    public void SetCollided()
    {
        if (onRush && !collided)
        {
            collided = true;
        }
    }

    private IEnumerator BullRush(int mult, Animator animator)
    {
        collided = false;
        onRush = true;
        animator.SetBool("Rush", true);
        while (!collided)
        {
            //move
            rb.MovePosition(transform.position);
            Vector2 targetPos = new Vector2(transform.position.x + 0.1f * mult, transform.position.y);
            rb.MovePosition(targetPos);
            yield return null;
        }
        onRush = false;
        collided = false;
        rb.MovePosition(transform.position);
        animator.SetBool("Rush", false);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Character"))
        {
            CharacterInfo playerInfo = _player.GetComponent<CharacterInfo>();
            playerInfo.TakeDamage(25f);
            SetCollided();
            
        }
    }

    internal void TriggerFoodThrow()
    {
        StartCoroutine(FoodThrow());
    }

    private IEnumerator FoodThrow()
    {

        //throw the food
        GameObject food = Instantiate(foodPrefab, firingPoint.position, firingPoint.rotation);
        SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerBullet, 1f);

        //TODO
       // There are situations where the player can hit the boss, but the boss doens't it back

        yield return null;
    }
}
