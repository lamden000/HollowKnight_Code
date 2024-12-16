using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingHuskAttackPrepare : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = animator.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.transform.position - rb.transform.position).normalized;
        if (direction.x < 0)
        {
            animator.SetInteger("moveDirection", -1);
            animator.transform.localScale = new Vector3(1, animator.transform.localScale.y, animator.transform.localScale.z);
        }
        else
        {
            animator.SetInteger("moveDirection", 1);
            animator.transform.localScale = new Vector3(-1, animator.transform.localScale.y, animator.transform.localScale.z);
        }
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
