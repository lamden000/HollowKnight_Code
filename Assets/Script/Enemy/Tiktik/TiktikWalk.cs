using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TiktikWalk : StateMachineBehaviour
{
    TiktikScript tiktik;
    Rigidbody2D rb;
    private int currentPointIndex = 0;  // Index of the current point the enemy is moving towards
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        tiktik =animator.GetComponent<TiktikScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveAlongPoints();
    }

    void MoveAlongPoints()
    {
        Vector2 targetPoint = tiktik.points[currentPointIndex];  // Get the current point to move towards
        Vector2 direction = targetPoint - (Vector2)rb.transform.position;

        // Move the enemy towards the current point
        rb.transform.position = Vector2.MoveTowards(rb.transform.position, targetPoint, tiktik.moveSpeedX * Time.deltaTime);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.transform.rotation = Quaternion.Euler(0, 0, angle);
        // Check if the enemy has reached the current point
        if (Vector2.Distance(rb.transform.position, targetPoint) < 0.1f)
        {
            // Move to the next point
            currentPointIndex++;

            // Loop back to the first point if all points are visited
            if (currentPointIndex >= tiktik.points.Length)
            {
                currentPointIndex = 0;
            }
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
