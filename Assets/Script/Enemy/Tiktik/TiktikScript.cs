using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TiktikScript : EnemyBase
{
	public Vector2[] points;  // Array of manually input points
	public float moveSpeed = 2f;   // Speed at which the enemy moves
	private int currentPointIndex = 0;  // Index of the current point the enemy is moving towards

	public float deathForce = 200f;
	public LayerMask surfaceLayer;
	// Start is called before the first frame update
	enum TiktikState
    {
        Walking_left = 0,
        Walking_right = 1,
        Dead = -1,

    }

    protected override void Start()
    {
        base.Start();
        health = 50;
        attackPower = 5;
        SetState(0); // Walk left
		if (points.Length > 0)
		{
            transform.position = points[0];
		}
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
             //   rb.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 1:
             //   rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
        /*
            WALK_LEFT == 0
            WALK_RIGHT == 1
            WAS ATTACK LEFT == 2
            WAS ATTACK RIGHT == 3
            DEAD == -1
        */
        switch (state)
        {
            case (int)TiktikState.Walking_left:
                nx = -1;            
                break;
            case (int)TiktikState.Walking_right:              
                nx = 1;
                break;
            case 2:
                nx = -1;
                break;
            case 3:
                nx = 1;
                break;
            case (int)TiktikState.Dead:
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
        SetState((int)TiktikState.Dead);

        rb.velocity = Vector2.zero;
		rb.AddForce(attackDirection.normalized * deathForce);
		rb.gravityScale = 1;

		Destroy(gameObject, 3);
    }

	public override void Attack()
    {
        Debug.Log("Crawlid attacks by colliding with the player.");
    }

    public override void Move()
	{
		if (points.Length > 0)
		{
			MoveAlongPoints();
		}
	}
	void MoveAlongPoints()
	{
		Vector2 targetPoint = points[currentPointIndex];  // Get the current point to move towards
		Vector2 direction = targetPoint - (Vector2)transform.position;

		// Move the enemy towards the current point
		transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
		// Check if the enemy has reached the current point
		if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
		{
			// Move to the next point
			currentPointIndex++;

			// Loop back to the first point if all points are visited
			if (currentPointIndex >= points.Length)
			{
				currentPointIndex = 0;
			}
		}
	}

	// Optional: Visualize the points in the Scene view
	void OnDrawGizmos()
	{
		// Draw points as small spheres and lines between points
		if (points != null && points.Length > 0)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.color = Color.green;
				// Draw lines between consecutive points
				if (i > 0)
				{
					Gizmos.DrawLine(points[i - 1], points[i]);
				}
				else
                {
					Gizmos.color = Color.red;
				}

				if (i == points.Length - 1)
				{
					Gizmos.DrawLine(points[0], points[i]);
				}
				Gizmos.DrawSphere(points[i], 0.1f);

			}
		}
	}
}
