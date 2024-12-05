using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingHuskIdle : StateMachineBehaviour
{
    private float idleTime;
    private WanderingHusk wanderingHusk;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime = 0;
        wanderingHusk=animator.GetComponent<WanderingHusk>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTime += Time.deltaTime;
        if(wanderingHusk.IsPlayerInAttackZone())
        {
            animator.SetTrigger("attack");
        }
        else if (idleTime > wanderingHusk.idleTime)
            animator.SetTrigger("walk");
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
