using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : EnemyInfo
{
    public float speed = 2f;

    public float detectionRange = 2f; //default range
    public float attackRange = 0.5f;
    public float attackDamage = 10;
    public float attackSpeed = 0.5f;
    public float health = 20f;

    private bool attackOnCoolDown = false;

    public Animator snakeAnimator;

    [HideInInspector]
    public GameObject target;   // the target i want to get closer to 
    [SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = .02f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private LayerMask m_WhatIsGround;
    private bool m_Grounded;            // Whether or not the player is grounded.


    private bool dead = false;

    private bool m_FacingRight = false;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Character");
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                rb.MovePosition(transform.position);
                break;
            }
        }

        float distToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distToPlayer <= attackRange)
        {
            StartCoroutine(Attack());
        } else if (m_Grounded && distToPlayer <= detectionRange)
        {
            follow(target.transform);

        }


    }

    private void follow(Transform target)
    {
        Vector2 targetPos = new Vector2(target.position.x, transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);

        float xDiff = target.position.x - transform.position.x;
        // If the input is moving the player right and the player is facing left...
        if (xDiff > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (xDiff < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

        if (m_Grounded)
        {

            snakeAnimator.SetFloat("Speed", Mathf.Abs(xDiff));
        } else
        {
            snakeAnimator.SetFloat("Speed", 0f);
        }
        rb.MovePosition(newPos);

    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0, 180, 0);
    }

    private IEnumerator Attack()
    {

        if (!attackOnCoolDown)
        {
            attackOnCoolDown = true;
            CharacterInfo character = target.gameObject.GetComponent<CharacterInfo>();

            character.TakeDamage(attackDamage);
            snakeAnimator.SetTrigger("Attack");

            yield return new WaitForSeconds(attackSpeed);
            attackOnCoolDown = false;
        }


    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.001)
        {
            StartCoroutine(Die());
        } else
        {
            snakeAnimator.SetTrigger("Hurt");
        }
    }

    private IEnumerator Die()
    {
        dead = true;
        snakeAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(10f);
        GameObject.Destroy(gameObject);
    }
}
