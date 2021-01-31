using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyIdleAction : StateMachineBehaviour
{
    public float range = 10f;
    public float delayForAttack = 2f;
    public float timeSinceLastAttck = 0f;

    private Transform _player;
    private Rigidbody2D rb;
    private int layerMask;

    private void OnEnable()
    {
        layerMask = ~LayerMask.GetMask("Enemy");
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Character").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float targetX = _player.position.x;
        if (targetX > rb.position.x)
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        //Do not try to attack if you just entered idle
        if (Time.realtimeSinceStartup - this.timeSinceLastAttck < this.delayForAttack) return;

        if (Vector3.Distance(_player.position,rb.position) <= range)
        {
            GluttonyEnemy enemyController = animator.GetComponent<GluttonyEnemy>();

            if (rb.position.y - _player.position.y  < -0.3f)
            {
                //The player is above me. I want to throw food at him!
                RaycastHit2D hit = Physics2D.Raycast(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y, 0f)), range, layerMask);
                if (hit.collider != null)
                {
                    CharacterInfo characterInfo = hit.collider.GetComponent<CharacterInfo>();
                    if (characterInfo != null && hit.distance < range)
                    {
                     
                        enemyController.TriggerFoodThrow(animator);
                        timeSinceLastAttck = Time.realtimeSinceStartup;
                    }
                }
                
            } else
            {
                //the player is standing on the ground or about to stand. Bull rush him
                enemyController.TriggerBullRush(animator);
                timeSinceLastAttck = Time.realtimeSinceStartup;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Super");
    }
}
