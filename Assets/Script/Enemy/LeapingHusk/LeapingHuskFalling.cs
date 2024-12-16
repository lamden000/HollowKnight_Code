using UnityEngine;

public class EnemyFall : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private LeapingHuskScript enemyScript;
    private float t; // Parabola time value
    private bool isFalling; // To track if the enemy is falling after the parabola peak

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        enemyScript = animator.GetComponent<LeapingHuskScript>();

        t = 0.5f; // Start from the midpoint of the parabola
        isFalling = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isFalling)
        {
            // Calculate the parabola points while ascending
            t += Time.deltaTime / enemyScript.attackDuration;
            if (t >= 1f)
            {
                t = 1f; // Clamp to 1 when the parabola is completed
                isFalling = true; // Start falling after reaching the peak
            }

            Vector2 nextPosition = LeapingHuskScript.GetParabolaPoint(enemyScript.startPosition, enemyScript.targetPosition, enemyScript.jumpHeight, t);
            rb.MovePosition(nextPosition);
        }

        if (enemyScript.IsGrounded())
        {
            animator.SetTrigger("isGrounded"); // Transition to grounded state
        }
    }
}