using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private Transform _canvas;
    public GameObject healthBarPrefab;
    private HealthBarController _healthBar;
    private float maxHealth;
    private Transform _player;
    private int layerMask;

    private Coroutine bullRushCoroutine;

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
            GameObject healthBarObject = Instantiate(healthBarPrefab, _canvas);
            _healthBar = healthBarObject.GetComponent<HealthBarController>();
            healthBarObject.transform.Find("BossName").GetComponent<TextMeshProUGUI>().text = "Gluttony";
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
        SoundManager.Instance.PlaySound(SoundManager.Sound.GluttonyDead, 1f);
        StopAllCoroutines();
        rb.MovePosition(transform.position); // stop movign


        transform.GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.EndLevel();
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GluttonyHurt, 0.2f);
        float percentage = this.health / this.maxHealth;
        percentage = percentage < 0 ? 0 : percentage;
        _healthBar.UpdateHealthBar(percentage);
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

        bullRushCoroutine = StartCoroutine(BullRush(mult,animator));

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
        SoundManager.Instance.PlaySound(SoundManager.Sound.GluttonyRush, 0.5f);
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
        SoundManager.Instance.PlaySound(SoundManager.Sound.GluttonyAtack, 0.3f);

        //TODO
       // There are situations where the player can hit the boss, but the boss doens't it back

        yield return null;
    }

    public void TriggerHpRegen(Animator animator)
    {
        StartCoroutine(HpRegen(animator));
    }

    private IEnumerator HpRegen(Animator animator)
    {
        animator.SetBool("HpRegen", true);
        gluttonySuperParticleEffect.SetActive(true);

        if(bullRushCoroutine!=null) StopCoroutine(bullRushCoroutine);

        rb.MovePosition(transform.position); // stop movign
        yield return new WaitForSeconds(1.5f);

        health += 200;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthBar();
  
        yield return new WaitForSeconds(1f);


        gluttonySuperParticleEffect.SetActive(false);
        animator.SetBool("HpRegen", false);

    }
}
