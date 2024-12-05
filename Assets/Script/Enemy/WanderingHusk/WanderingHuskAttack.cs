using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingHuskAttack : StateMachineBehaviour
{
    private float attackTime;
    private WanderingHusk wanderingHusk;
    private GameObject player;

    private Transform enemyTransform;
    private Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        attackTime = 0;
        wanderingHusk = animator.GetComponent<WanderingHusk>();
        player = GameObject.FindGameObjectWithTag("Player");
        // Initialize the Rigidbody and Transform
        rb = animator.GetComponent<Rigidbody2D>();
        enemyTransform = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTime += Time.deltaTime;
        int moveDirection =animator.GetInteger("moveDirection");
        float targetVelocityX = moveDirection * wanderingHusk.moveSpeedX*2;

        // Smoothly interpolate the current velocity towards the target velocity
        float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, wanderingHusk.accelerationX * Time.fixedDeltaTime);

        // Set the Rigidbody's velocity
        rb.velocity = new Vector2(newVelocityX, rb.velocity.y);

        if (attackTime > wanderingHusk.attackTime)
            animator.SetTrigger("attackCooldown");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
