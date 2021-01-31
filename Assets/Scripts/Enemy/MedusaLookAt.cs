using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaLookAt : StateMachineBehaviour
{
    public float range = 10f;
    public float minDelay = 0.75f;
    public float maxDelay = 1.75f;
    public float timeSinceLastAttck = 0f;

    private Transform _player;
    private Rigidbody2D rb;
    private int layerMask;
    private float _delayForAttack = 1.25f;

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
        if(targetX > rb.position.x)
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }

        //Do not try to attack if you just entered idle
        if (Time.realtimeSinceStartup - this.timeSinceLastAttck < this._delayForAttack) return;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y, 0f)), range, layerMask);
        if(hit.collider != null)
        {
            CharacterInfo characterInfo = hit.collider.GetComponent<CharacterInfo>();
            //Debug.DrawRay(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y,0f)), Color.yellow);
            if (characterInfo != null && hit.distance < range)
            {
                //Debug.DrawRay(rb.position, (_player.position - new Vector3(rb.position.x, rb.position.y, 0f)), Color.red);
                animator.GetComponent<EnemyController>().TriggerAttack(animator);
                timeSinceLastAttck = Time.realtimeSinceStartup;
                _delayForAttack = Random.Range(this.minDelay, this.maxDelay);
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
