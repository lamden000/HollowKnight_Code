using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WanderingHuskWalk : StateMachineBehaviour
{
    private float walkRestTime;
    private WanderingHusk wanderingHusk;

    private Transform enemyTransform;
    private Rigidbody2D rb;
    public float raycastDistance = 1f;     // Distance of the ray to detect the edge
    public LayerMask groundLayer;          // Layer mask for ground detection
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkRestTime = 0;
        wanderingHusk = animator.GetComponent<WanderingHusk>();
        // Initialize the Rigidbody and Transform
        rb = animator.GetComponent<Rigidbody2D>();
        enemyTransform = animator.transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkRestTime += Time.deltaTime;
        int moveDirection = animator.GetInteger("moveDirection");
        float targetVelocityX = moveDirection * wanderingHusk.moveSpeedX;

        // Smoothly interpolate the current velocity towards the target velocity
        float newVelocityX = Mathf.MoveTowards(rb.velocity.x, targetVelocityX, wanderingHusk.accelerationX * Time.fixedDeltaTime);

        // Set the Rigidbody's velocity
        rb.velocity = new Vector2(newVelocityX, rb.velocity.y);

        if (wanderingHusk.IsPlayerInAttackZone())
        {
            animator.SetTrigger("attack");
        }
        else if (walkRestTime > wanderingHusk.walkRestTime)
            animator.SetTrigger("idle");

        if (IsBlocked(animator) || IsOnVerge(animator))
        {
            // If the enemy hits something with the "platform" tag, flip direction
            animator.SetTrigger("turn");
        }
    }
    private bool IsOnVerge(Animator animator)
    {
        // Cast a ray downwards from the enemy's front
        Vector2 rayOrigin = new Vector2(enemyTransform.position.x + (animator.GetInteger("moveDirection") * 0.1f), enemyTransform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);

        // Debug ray to visualize it in Scene view
        Debug.DrawRay(rayOrigin, Vector2.down * raycastDistance, Color.red);

        // If the ray doesn't hit anything, we're at the verge
        return hit.collider == null;
    }

    private bool IsBlocked(Animator animator)
    {
        // Cast a ray downwards from the enemy's front
        Vector2 rayOrigin = new Vector2(enemyTransform.position.x + (animator.GetInteger("moveDirection") * 0.1f), enemyTransform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, new Vector2(animator.GetInteger("moveDirection"),0), raycastDistance, groundLayer);

        // Debug ray to visualize it in Scene view
        Debug.DrawRay(rayOrigin, new Vector2(animator.GetInteger("moveDirection"),0) * raycastDistance, Color.red);

        // If the ray doesn't hit anything, we're at the verge
        return hit.collider != null;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //     
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
