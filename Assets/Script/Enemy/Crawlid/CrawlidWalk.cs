using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlidWalk : StateMachineBehaviour
{
    private Crawlid crawlid;
    private Vector2 moveDirection; 
    private Transform enemyTransform;
    private Rigidbody2D rb;
    public float raycastDistance = 1f;     // Distance of the ray to detect the edge
    public LayerMask groundLayer;          // Layer mask for ground detection


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Initialize the Rigidbody and Transform
        rb = animator.GetComponent<Rigidbody2D>();
        crawlid=animator.GetComponent<Crawlid>();
        enemyTransform = animator.transform;

        // Set the initial movement direction (right or left)
        moveDirection = Vector2.left;
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(moveDirection.x * crawlid.moveSpeedX, rb.velocity.y);

        if (IsBlocked() || IsOnVerge())
        {
            // If the enemy hits something with the "platform" tag, flip direction
            moveDirection *= -1;
            enemyTransform.localScale = new Vector3(-enemyTransform.localScale.x, enemyTransform.localScale.y, enemyTransform.localScale.z);
        }
    }

    private bool IsOnVerge()
    {
        // Cast a ray downwards from the enemy's front
        Vector2 rayOrigin = new Vector2(enemyTransform.position.x + (moveDirection.x * 0.5f), enemyTransform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);

        // Debug ray to visualize it in Scene view
        Debug.DrawRay(rayOrigin, Vector2.down * raycastDistance, Color.red);

        // If the ray doesn't hit anything, we're at the verge
        return hit.collider == null;
    }

    private bool IsBlocked()
    {
        // Cast a ray downwards from the enemy's front
        Vector2 rayOrigin = new Vector2(enemyTransform.position.x + (moveDirection.x * 0.5f), enemyTransform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, moveDirection, raycastDistance, groundLayer);

        // Debug ray to visualize it in Scene view
        Debug.DrawRay(rayOrigin, moveDirection * raycastDistance, Color.red);

        // If the ray doesn't hit anything, we're at the verge
        return hit.collider != null;
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
