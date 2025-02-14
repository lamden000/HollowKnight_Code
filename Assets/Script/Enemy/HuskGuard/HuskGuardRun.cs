using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskGuardRun : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private HuskGuardScript guard;
    [SerializeField] private float shakeIntensity = 1.0f;
    [SerializeField] private float shakeDuration = 0.1f;
    private bool isShaking;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isShaking)
        {
            isShaking = true;
            CameraShake.instance?.ShakeCamera(shakeIntensity, shakeDuration);
        }
        rb = animator.GetComponent<Rigidbody2D>();
        guard = animator.GetComponent<HuskGuardScript>();
        player = guard.player;
    } 

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            animator.SetTrigger("fightingIdle");
        }
        else
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);  // Target on the same Y as the Rigidbody
            Vector2 direction = target - rb.position;  // Calculate the direction towards the target

            // Normalize the direction vector to get a unit vector, and multiply by the desired speed
            Vector2 velocity = direction.normalized * guard.moveSpeedX * guard.runSpeedMultify;

            // Set the Rigidbody2D's velocity
            rb.velocity = new Vector2(velocity.x, rb.velocity.y);  // Keep the current Y velocity (if you want to maintain vertical movement)
            if (direction.x > 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
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

                    if (Random.Range(0, 100) > 30)
                    {
                        animator.SetTrigger("jumpAttack");
                    }
                    else
                    {
                        animator.SetTrigger("normalAttack");
                    }
                }
            }
            CameraShake.instance?.ShakeCamera(shakeIntensity, shakeDuration);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        if (isShaking)
        {
            CameraShake.instance?.ShakeCamera(0f, 0f); // Reset shake
            isShaking = false;
        }
        rb.velocity=new Vector2(0, rb.velocity.y);
    }

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
