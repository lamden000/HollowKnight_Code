using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskGuardFightingIdle : StateMachineBehaviour
{
    HuskGuardScript guard;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard=animator.GetComponent<HuskGuardScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(guard.IsPlayerInSecurityZone())
        {
            if (guard.IsPlayerInAttackZone())
            {
                animator.SetTrigger("normalAttack");
            }
            else
            {
                if(Random.Range(0,2) == 0)
                {
                    animator.SetTrigger("run");
                }
                else
                {
                    animator.SetTrigger("jumpAttack");
                }
            }
        }
        else
        {
            guard.StartCoroutine(WaitMakeSure(animator));
        }
    }

    IEnumerator WaitMakeSure(Animator animator)
    {
        yield return new WaitForSeconds(3f);
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
