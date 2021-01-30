using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmacEnemy : EnemyInfo
{
    public float speed = 2f;

    public float detectionRange = 2f; //default range
    public float attackRange = 0.5f;
    public float attackDamage = 10;
    public float attackSpeed = 0.5f;
    public float health = 20f;

    private bool attackOnCoolDown = false;

    public Animator batmacAnimator;

    [HideInInspector]
    public bool hasTarget = false;  // do I have a target to move towards
    [HideInInspector]
    public GameObject target;   // the target i want to get closer to 

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

        float distToPlayer = Vector3.Distance(target.transform.position, transform.position);

        if (distToPlayer <= attackRange)
        {
            StartCoroutine(Attack());
        } else if (distToPlayer <= detectionRange)
        {
            follow(target.transform);

        }


    }

    private void follow(Transform target)
    {
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
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
            batmacAnimator.SetTrigger("Attack");

            yield return new WaitForSeconds(attackSpeed);
            attackOnCoolDown = false;
        }


    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,detectionRange);
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0.001)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        dead = true;
        batmacAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(10f);
        GameObject.Destroy(gameObject);
    }
}
