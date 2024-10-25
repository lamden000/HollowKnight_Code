using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskGuardRun : StateMachineBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private HuskGuardScript guard;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = animator.GetComponent<Rigidbody2D>();
        guard = animator.GetComponent<HuskGuardScript>();
    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Vector2 target=new Vector2(player.transform.position.x, rb.transform.position.y);
       Vector2 newPos=Vector2.MoveTowards(rb.transform.position, target, guard.runSpeed*Time.fixedDeltaTime);
       Vector2 direction = rb.position - target;
        rb.MovePosition(newPos);
        if (direction.x < 0)
        {
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            rb.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (!guard.IsPlayerInSecurityZone())
        {
            animator.SetTrigger("fightingIdle");
        }
        else
        {
            if (guard.IsPlayerInAttackZone())
            {
                animator.SetTrigger("normalAttack");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  //  {
   // }

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
