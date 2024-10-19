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
        Vector2 target = new Vector2(guard.startPos.x, rb.transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.transform.position, target, guard.walkSpeed * Time.fixedDeltaTime);
        Vector2 direction = rb.position - target;
        rb.MovePosition(newPos);
        if (direction.x < 0)
        {
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //  {

    //  }

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
