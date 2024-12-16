using UnityEngine;

public class EnemyJumpAscend : StateMachineBehaviour
{
    private Transform enemyTransform;
    private Rigidbody2D rb;
    private LeapingHuskScript enemyScript;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float jumpHeight;

    private float t; // Parameter for parabola interpolation

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyTransform = animator.transform;
        rb = enemyTransform.GetComponent<Rigidbody2D>();
        enemyScript = enemyTransform.GetComponent<LeapingHuskScript>();

        // Initialize positions
        startPosition = enemyTransform.position;
        targetPosition = enemyScript.player.transform.position + new Vector3(0, -1,0);//add player height/2
        enemyScript.targetPosition= targetPosition;
        enemyScript.startPosition = startPosition;
        jumpHeight = enemyScript.jumpHeight;

        // Calculate parabola points (for visualization and movement)
        enemyScript.parabolaPoints = CalculateParabolaPoints(startPosition, targetPosition , jumpHeight, 30); // 30 segments
        enemyScript.drawParabola = true;

        t = 0f; // Reset interpolation parameter
        animator.SetBool("isGrounded", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Use the attack duration to calculate t
        t += Time.deltaTime / enemyScript.attackDuration; // Normalize over the duration of the jump
        if (t >= 1f)
        {
            t = 1f; // Ensure it doesn't go past 1
        }

        // Calculate the next position along the parabola
        Vector2 nextPosition = GetParabolaPoint(startPosition, targetPosition, jumpHeight, t);

        // Move the enemy to the next position along the parabola
        rb.MovePosition(nextPosition);

        // Transition to falling phase at the parabola's peak (midpoint)
        if (t >= 0.5f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            animator.SetTrigger("fall"); // Trigger fall animation
        }
    }

    // Calculate the parabola points for visualization
    private Vector2[] CalculateParabolaPoints(Vector2 start, Vector2 end, float height, int segments)
    {
        Vector2[] points = new Vector2[segments];
        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            points[i] = GetParabolaPoint(start, end, height, t);
        }
        return points;
    }

    // Get a single point on the parabola
    private Vector2 GetParabolaPoint(Vector2 start, Vector2 end, float height, float t)
    {
        float x = Mathf.Lerp(start.x, end.x, t); // Interpolate X coordinate
        float y = Mathf.Lerp(start.y, end.y, t) + height * 4 * t * (1 - t); // Parabola formula for Y (quadratic shape)
        return new Vector2(x, y);
    }
}