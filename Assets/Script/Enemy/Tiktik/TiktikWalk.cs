using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TiktikWalk : StateMachineBehaviour
{
    TiktikScript tiktik;
    Rigidbody2D rb;
    public LayerMask groundLayer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        tiktik =animator.GetComponent<TiktikScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Get the current rotation (in degrees)
        float currentZRotation = rb.transform.rotation.eulerAngles.z;

        // Calculate the direction based on current rotation
        Vector2 forwardDirection = (Quaternion.Euler(0, 0, currentZRotation) * Vector2.right).normalized;  // Direction facing to the right
        Vector2 downDirection = (Quaternion.Euler(0, 0, currentZRotation) * Vector2.down).normalized;      // Direction downward

        // Calculate the head position based on current rotation
        Vector2 headPositionOffset = (Quaternion.Euler(0, 0, currentZRotation) * Vector2.right) *0.4f;  // Adjust 0.5f for distance from center to head
        Vector2 raycastStartPosition = rb.position + headPositionOffset;  // Position of head relative to object center

        // Cast the ray in the direction the object is facing (downward relative to its rotation)
        RaycastHit2D hit = Physics2D.Raycast(raycastStartPosition, downDirection, 0.4f, groundLayer);
        Debug.DrawRay(raycastStartPosition, downDirection * 0.4f, Color.red);

        // Check if the ray did not hit anything (no wall below)
        if (hit.collider == null)
        {
            // Slowly rotate the object by a small step each frame
            float targetRotation = currentZRotation - 40f; // Rotate by 40 degrees per frame

            // Smoothly rotate towards the target rotation
            float newZRotation = Mathf.LerpAngle(currentZRotation, targetRotation, Time.deltaTime * 2f); // Smooth rotation speed multiplier

            // Apply the new rotation to the enemy (keep the x and y rotation at 0)
            rb.transform.rotation = Quaternion.Euler(0, 0, newZRotation);

            // Stop any movement if no ground is detected (because it's rotating)
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Move both forward and downward based on current rotation
            Vector2 moveDirection = forwardDirection * tiktik.moveSpeedX; // Horizontal (forward) movement
            Vector2 downMovement = downDirection * tiktik.moveSpeedY;     // Downward movement (scale as needed)

            // Set velocity: Combine forward and downward velocities
            rb.velocity = moveDirection + downMovement; // Move based on current rotation (forward + down)
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
