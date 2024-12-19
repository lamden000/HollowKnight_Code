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

    public float runSpeedMultify = 2f;

    public Vector2 startPos;
    public Vector2 jumpForce;

    public delegate void OnBossDeath();
    public static event OnBossDeath BossDeathEvent;

    // Start is called before the first frame update

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        base.Start();
    }

    void Update()
    {
    }

    public void Jump()
    {
        int direction=transform.rotation.y==0 ? -1 : 1;
        rb.AddForce(new Vector2(direction*jumpForce.x,jumpForce.y),ForceMode2D.Impulse);
    }

    protected override void Die(int attackDirection)
    {
        base.Die(attackDirection);
        if (BossDeathEvent != null)
        {
            BossDeathEvent.Invoke();
            CameraShake.instance.ShakeCamera(5f, 12f);
        }
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
        if(player==null)
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

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            int direction = other.transform.position.x - transform.position.x > 0 ? 1 : -1;
            other.GetComponent<PlayerScript>().TakeDamage(attackPower,direction);
        }
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

