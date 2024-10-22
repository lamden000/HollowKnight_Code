using Microsoft.Azure.Cosmos.Spatial;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HuskGuardScript : EnemyBase
{
    //if the player go into the zone, the guard will attack
    public float zoneHeight;
    public float zoneWidth;

    public float wakeUpWidth;

    public float normalAttackHeight;
    public float normalAttackWidth;

    public Vector3 zoneCenter;
    public Color zoneColor = Color.green;
    public Color wakeUpColor = Color.blue;
    public Color normalAttackColor=Color.red;

    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float deathForce = 200f;

    public Transform player;
    public Vector2 startPos;

    // Start is called before the first frame update
    enum HuskGuardState
    {
        Walking_left = 0,
        Walking_right = 1,
        Waiting=2,
        Dead = -1,
    }

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        health = 50;
        attackPower = 5;
    }

    void Update()
    {

        if (GetState() == -1)
        {
            return;
        }
        Move();
        Render();
    }
    public override void Render()
    {
        switch (state)
        {
            case 0:
                spriteRenderer.flipX = false;
                break;
            case 1:
                break;
            case 2:
                nx = -1;
                break;
            case 3:

                break;
            case -1:
                animator.SetTrigger("Dead");
                //StartCoroutine(WaitForAnimation(animator, "Dead"));
                break;
        }
    }
    public override void SetState(int state)
    {
        switch (state)
        {
            case (int)HuskGuardState.Walking_left:
                nx = -1;
                break;
            case (int)HuskGuardState.Walking_right:
                nx = 1;
                break;
            case 2:
                nx = -1;
                break;
            case 3:
                nx = 1;
                break;
            case (int)HuskGuardState.Dead:
                Render();
                break;
            default:
                Debug.Log("Error SetState Crawlid");
                break;
        }

        base.SetState(state);
    }
    protected override void Die(Vector2 attackDirection)
    {
        animator.SetTrigger("Dead");
        SetState((int)HuskGuardState.Dead);
        rb.velocity = Vector2.zero;
        rb.AddForce(attackDirection.normalized * deathForce);
        Destroy(gameObject, 3);
    }
    public override void Move()
    {
       
    }

    public bool IsPlayerInWakeUpZone() { 
        
        if(IsPlayerInZone(wakeUpWidth,zoneHeight ,transform.position + new Vector3(0, 2, 0))) 
            return true;

        return false;
    }

    public bool IsPlayerInAttackZone()
    {

        if (IsPlayerInZone(normalAttackWidth, normalAttackHeight, transform.position + new Vector3(0, 2, 0)))
            return true;

        return false;
    }

    public bool IsPlayerInSecurityZone()
    {

        if (IsPlayerInZone(zoneWidth, zoneHeight, zoneCenter))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetState() == -1)
        {
            return;
        }
    }

    public override void Attack()
    {

    }

    private void OnDrawGizmos()
    {
        // Draw the security zone
        Gizmos.color = zoneColor;
        DrawRectangle(zoneWidth, zoneHeight, zoneCenter);

        // Draw the wake-up zone
        Gizmos.color = wakeUpColor;
        DrawRectangle(wakeUpWidth, zoneHeight, transform.position+new Vector3(0,2,0));

        //Draw the normal attack range
        Gizmos.color = normalAttackColor;
        DrawRectangle(normalAttackWidth, normalAttackHeight, transform.position + new Vector3(0, 2, 0));
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

