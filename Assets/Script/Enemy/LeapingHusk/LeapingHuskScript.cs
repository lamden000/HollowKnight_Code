using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingHuskScript : EnemyBase
{
    public float attackHeight;
    public float attackWidth;

    public float walkRestTime;
    public float idleTime;

    public float jumpHeight = 5f;     
    public float jumpSpeed = 2f;       
    public LayerMask groundLayer;

    public float attackDuration = 2.0f; // Default duration of the attack

    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public Vector2 targetPosition;

    public Vector2[] parabolaPoints;
    public bool drawParabola = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Vector2 GetParabolaPoint(Vector2 startPosition, Vector2 targetPosition, float jumpHeight, float t)
    {
        // Interpolating X and Y values separately
        float x = Mathf.Lerp(startPosition.x, targetPosition.x, t); // Linear interpolation of X
        float y = Mathf.Lerp(startPosition.y, targetPosition.y, t); // Linear interpolation of Y

        // Adjusting Y value to form a parabola
        y += jumpHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // Parabolic shape adjustment

        return new Vector2(x, y);
    }
    public bool IsGrounded()
    {
        // Check if the enemy is on the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
        return hit.collider != null;
    }

    public bool IsPlayerInAttackZone()
    {

        if (IsPlayerInZone(attackWidth, attackHeight, new Vector3(transform.position.x,transform.position.y+1,0)))
            return true;
        return false;
    }

    private bool IsPlayerInZone(float width, float height, Vector3 center)
    {
        if (player == null)
            return false;
        Vector3 playerPos = player.position;

        // Calculate bounds of the rectangle
        float minX = center.x - width / 2;
        float maxX = center.x + width / 2;
        float minY = center.y - height / 2;
        float maxY = center.y + height / 2;

        // Check if the player is inside the bounds
        return playerPos.x >= minX && playerPos.x <= maxX && playerPos.y >= minY && playerPos.y <= maxY;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position+Vector3.down*0.3f);
        // Draw the security zone
        Gizmos.color = Color.red;
        DrawRectangle(attackWidth, attackHeight, new Vector3(transform.position.x, transform.position.y + 1, 0));

        if (parabolaPoints != null && drawParabola)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < parabolaPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(parabolaPoints[i], parabolaPoints[i + 1]);
            }
        }
        else if(parabolaPoints == null&& drawParabola) { Debug.Log("no point"); }
        else if (parabolaPoints == null && drawParabola) { Debug.Log("no drawn"); }
    }


    private void DrawRectangle(float width, float height, Vector3 center)
    {
        Vector3 topLeft = new Vector3(center.x - width / 2, center.y + height / 2);
        Vector3 topRight = new Vector3(center.x + width / 2, center.y + height / 2);
        Vector3 bottomRight = new Vector3(center.x + width / 2, center.y - height / 2);
        Vector3 bottomLeft = new Vector3(center.x - width / 2, center.y - height / 2);

        // Draw the rectangle using lines between corners
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
