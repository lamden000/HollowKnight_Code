using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class VengeflyChase : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private VengeflyScript vengeflyScript;
    private GameObject player;
    private bool isFacingRight = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        vengeflyScript =animator.GetComponent<VengeflyScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = (player.transform.position - rb.transform.position).normalized;

        Vector2 targetVelocity = direction * vengeflyScript.moveSpeedX;

        rb.velocity = Vector2.MoveTowards(rb.velocity,targetVelocity,vengeflyScript.accelerationX);
        DetectPlayerDirection(animator);
    }
    private void DetectPlayerDirection(Animator animator)
    {
        float directionToPlayer = player.transform.position.x - rb.position.x;

        if (directionToPlayer > vengeflyScript.turnThreshold && !isFacingRight)
        {
            // Player is to the right, but enemy is facing left -> turn to the right
            Turn(true,animator);
        }
        else if (directionToPlayer < -vengeflyScript.turnThreshold && isFacingRight)
        {
            // Player is to the left, but enemy is facing right -> turn to the left
            Turn(false,animator);
        }
    }

    private void Turn(bool faceRight, Animator animator)
    {
        animator.SetBool("turn",true);
        Vector3 scale = rb.transform.localScale;
        scale.x = faceRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        rb.transform.localScale = scale;

        // Update the facing direction
        isFacingRight = faceRight;
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
