using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengeflyScript : EnemyBase
{
    public LayerMask obstacleLayer;          // Layer for walls/obstacles (e.g., "Obstacles")
    private Transform player;
    public float turnThreshold = 0.1f;

    override protected void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
       
    }


    // Called when the player enters the enemy's detection circle
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Perform a raycast to check if there's a clear line of sight
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Raycast to detect obstacles between enemy and player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

            if (hit.collider == null) // No obstacle blocking the view, start chasing
            {
                animator.SetTrigger("startChasing");                
            }
        }
    }

}