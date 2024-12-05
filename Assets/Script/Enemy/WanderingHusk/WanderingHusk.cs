using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WanderingHusk : EnemyBase
{
    private Transform player;

    public float walkRestTime;
    public float idleTime;
    public float attackTime;

    public float attackHeight;
    public float attackWidth;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPlayerInAttackZone()
    {

        if (IsPlayerInZone(attackWidth, attackHeight, transform.position ))
            return true;

        return false;
    }

    private bool IsPlayerInZone(float width, float height, Vector3 center)
    {
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
        // Draw the security zone
        Gizmos.color = Color.red;
        DrawRectangle(attackWidth, attackHeight, transform.position);
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
