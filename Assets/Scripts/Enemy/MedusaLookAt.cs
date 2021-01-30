using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaLookAt : StateMachineBehaviour
{
    public float range = 10f;
    public float delayForAttack = 1f;
    public float timeEntered;

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
        timeEntered = Time.realtimeSinceStartup;
        animator.ResetTrigger("Hurt");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float targetX = _player.position.x;
        if(targetX > rb.position.x)
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        //Do not try to attack if you just entered idle
        if (Time.realtimeSinceStartup - this.timeEntered < this.delayForAttack) return;


        RaycastHit2D hit = Physics2D.Raycast(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y, 0f)), range, layerMask);
        if(hit.collider != null)
        {
            CharacterInfo characterInfo = hit.collider.GetComponent<CharacterInfo>();
            //Debug.DrawRay(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y,0f)), Color.yellow);
            if (characterInfo != null )
            {
                //Debug.DrawRay(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y, 0f)), Color.red);
                SoundManager.Instance.PlaySound(SoundManager.Sound.MedusaLaser, 0.1f);
                animator.SetTrigger("Attack");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
