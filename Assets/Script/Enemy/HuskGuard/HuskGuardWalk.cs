using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskGuardWalk : StateMachineBehaviour
{
    HuskGuardScript guard;
    private Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard = animator.GetComponent<HuskGuardScript>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(guard.startPos.x, rb.position.y);  // Target position (same Y as the Rigidbody)
        Vector2 direction = target - rb.position;  // Calculate direction towards the target

        // Normalize the direction vector and multiply by the desired speed
        Vector2 velocity = direction.normalized * guard.moveSpeedX;

        // Set the Rigidbody's velocity to move towards the target
        rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        if (direction.x > 0)
        {
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if(guard.IsPlayerInSecurityZone())
        {
            animator.SetTrigger("run");
        }
        else if(Mathf.Abs(rb.position.x - guard.startPos.x) < 0.5f) 
        {
            animator.SetTrigger("idle");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

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
